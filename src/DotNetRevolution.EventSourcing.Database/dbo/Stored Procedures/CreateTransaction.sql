CREATE PROCEDURE [dbo].[CreateTransaction]
	  @transactionId				UNIQUEIDENTIFIER
	, @user							NVARCHAR(256)
	, @commandId					UNIQUEIDENTIFIER	
	, @commandTypeId				BINARY(16)	
    , @commandTypeFullName			VARCHAR(512)	
    , @commandData					VARBINARY(MAX)
	, @eventProviderGuid			UNIQUEIDENTIFIER
	, @eventProviderId				UNIQUEIDENTIFIER
    , @eventProviderTypeId			BINARY(16)		
	, @eventProviderTypeFullName	VARCHAR (512)   
	, @eventProviderDescriptor		VARCHAR (MAX)   
	, @events						dbo.udttEvent READONLY	    
AS
BEGIN
	SET NOCOUNT ON;
					
	BEGIN TRAN 
		
	BEGIN TRY
				
		-- event provider type
		INSERT INTO dbo.EventProviderType (EventProviderTypeId, FullName)
		SELECT @eventProviderTypeId, @eventProviderTypeFullName
		 WHERE NOT EXISTS (SELECT EventProviderTypeId
							 FROM dbo.EventProviderType
					 		WHERE FullName = @eventProviderTypeFullName)
							
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
		
		-- event provider
		INSERT INTO dbo.EventProvider (EventProviderGuid, EventProviderId, EventProviderTypeId)				
		SELECT @eventProviderGuid, @eventProviderId, @eventProviderTypeId
		 WHERE NOT EXISTS (SELECT EventProviderGuid
							 FROM dbo.EventProvider
							WHERE EventProviderGuid = @eventProviderGuid)
								
		-- event provider descriptor
		MERGE dbo.EventProviderDescriptor AS target
		USING (VALUES (@eventProviderGuid, @eventProviderDescriptor)) AS source (EventProviderGuid, Descriptor)
		   ON target.EventProviderGuid = source.EventProviderGuid
		WHEN NOT MATCHED THEN
			INSERT (EventProviderGuid, Descriptor) 
			VALUES (source.EventProviderGuid, source.Descriptor)
		WHEN MATCHED THEN
			UPDATE SET Descriptor = source.Descriptor;
				
		-- create transaction
		INSERT INTO dbo.[EventProviderTransaction] (EventProviderTransactionId, EventProviderGuid, EventProviderVersion) 
		SELECT @transactionId, @eventProviderGuid, e.EventProviderVersion
		  FROM @events e

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
												AND ept.EventProviderGuid = @eventProviderGuid
		  
		IF @@ROWCOUNT = 0		
		BEGIN
			; THROW 51000, 'No transaction events were written.', 1;
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