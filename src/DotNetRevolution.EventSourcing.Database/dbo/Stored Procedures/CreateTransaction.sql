CREATE PROCEDURE [dbo].[CreateTransaction]
	  @transactionId				UNIQUEIDENTIFIER
	, @user							NVARCHAR(256)
	, @commandId					UNIQUEIDENTIFIER	
	, @commandTypeId				BINARY(16)	
    , @commandTypeFullName			VARCHAR(512)	
    , @commandData					VARBINARY(MAX)
	, @eventProviderId				UNIQUEIDENTIFIER
	, @aggregateRootId				UNIQUEIDENTIFIER
    , @aggregateRootTypeId			BINARY(16)		
	, @aggregateRootTypeFullName	VARCHAR (512)   
	, @eventProviderDescriptor		VARCHAR (MAX)   
	, @events						dbo.udttEvent READONLY	    
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @errorNum INT
					
	BEGIN TRAN 
		
	BEGIN TRY
				
		-- event provider type
		INSERT INTO dbo.AggregateRootType (AggregateRootTypeId, FullName)
		SELECT @aggregateRootTypeId, @aggregateRootTypeFullName
		 WHERE NOT EXISTS (SELECT AggregateRootTypeId
							 FROM dbo.AggregateRootType
					 		WHERE FullName = @aggregateRootTypeFullName)
							
		-- command type					
		INSERT INTO dbo.TransactionCommandType (TransactionCommandTypeId, FullName) 
		SELECT @commandTypeId, @commandTypeFullName
		WHERE NOT EXISTS (SELECT TransactionCommandTypeId 
							FROM dbo.TransactionCommandType
					 		WHERE FullName = @commandTypeFullName)

		-- insert transaction event types  
		INSERT INTO dbo.TransactionEventType (TransactionEventTypeId, FullName)
		SELECT e.TypeId, e.TypeFullName
		  FROM @events e
	 LEFT JOIN dbo.TransactionEventType tet ON e.TypeFullName = tet.FullName		
		 WHERE tet.TransactionEventTypeId IS NULL
		
		BEGIN TRY	

			-- event provider
			INSERT INTO dbo.EventProvider (EventProviderId, AggregateRootId, AggregateRootTypeId)				
			SELECT @eventProviderId, @aggregateRootId, @aggregateRootTypeId
			 WHERE NOT EXISTS (SELECT EventProviderId
								 FROM dbo.EventProvider
								WHERE EventProviderId = @eventProviderId)
								
			-- event provider descriptor
			MERGE dbo.EventProviderDescriptor AS target
			USING (VALUES (@eventProviderId, @eventProviderDescriptor)) AS source (EventProviderId, Descriptor)
			   ON target.EventProviderId = source.EventProviderId
			WHEN NOT MATCHED THEN
				INSERT (EventProviderId, Descriptor) 
				VALUES (source.EventProviderId, source.Descriptor)
			WHEN MATCHED THEN
				UPDATE SET Descriptor = source.Descriptor;
				

			-- create transaction
			INSERT INTO dbo.[EventProviderTransaction] (EventProviderTransactionId, EventProviderId, EventProviderVersion) 
			SELECT @transactionId, @eventProviderId, e.EventProviderVersion
				FROM @events e

		END TRY
		BEGIN CATCH		
			SET @errorNum = ERROR_NUMBER()

			IF @errorNum IN (2627,2601)
				THROW 51000, 'Concurrency exception with event provider.', 1;	
			ELSE
				THROW
			
		END CATCH

		-- insert transaction information
		INSERT INTO dbo.[TransactionInformation] (EventProviderTransactionId, [User])
		VALUES (@transactionId, @user)
			
		-- insert command
		INSERT INTO dbo.TransactionCommand (EventProviderTransactionId, TransactionCommandTypeId, TransactionCommandId, [Data])
		VALUES (@transactionId, @commandTypeId, @commandId, @commandData)
						
		-- insert transaction events
		INSERT INTO dbo.TransactionEvent (EventProviderTransactionId, TransactionEventTypeId, TransactionEventId, [Sequence], [Data])
		SELECT ept.EventProviderTransactionId, e.TypeId, e.EventId, e.[Sequence], e.[Data]
		  FROM @events e
		  JOIN dbo.EventProviderTransaction ept ON  e.EventProviderVersion = ept.EventProviderVersion
												AND ept.EventProviderId = @eventProviderId
		  
		IF @@ROWCOUNT = 0		
		BEGIN
			; THROW 51001, 'No transaction events were written.', 1;
		END

		-- nothing failed
		COMMIT TRAN 
		
		-- return success to caller
		RETURN 0
	END TRY

	BEGIN CATCH
		-- something failed
		ROLLBACK TRAN;
		
		-- raise error to caller
		THROW

		-- return failure to caller
		RETURN 1

	END CATCH
END