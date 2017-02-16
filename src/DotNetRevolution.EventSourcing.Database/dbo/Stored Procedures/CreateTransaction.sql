CREATE PROCEDURE [dbo].[CreateTransaction]
	  @transactionId				UNIQUEIDENTIFIER	
	, @metadata						VARBINARY(MAX)
	, @commandId					UNIQUEIDENTIFIER	
	, @commandTypeId				BINARY(16)	
    , @commandTypeFullName			VARCHAR(512)	
    , @commandData					VARBINARY(MAX)
	, @eventProviderId				UNIQUEIDENTIFIER
	, @aggregateRootId				UNIQUEIDENTIFIER
    , @aggregateRootTypeId			BINARY(16)		
	, @aggregateRootTypeFullName	VARCHAR(512)   
	, @eventProviderDescriptor		VARCHAR(MAX)   
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
		INSERT INTO dbo.EventProviderEventType (EventProviderEventTypeId, FullName)
		SELECT e.TypeId, e.TypeFullName
		  FROM @events e
	 LEFT JOIN dbo.EventProviderEventType epet ON e.TypeFullName = epet.FullName		
		 WHERE epet.EventProviderEventTypeId IS NULL
		
		-- insert transaction
		INSERT INTO dbo.[Transaction] (TransactionId, [Metadata], EventProviderDescriptor)
		VALUES (@transactionId, @metadata, @eventProviderDescriptor)
			
		-- insert command
		INSERT INTO dbo.TransactionCommand (TransactionId, TransactionCommandTypeId, TransactionCommandId, [Data])
		VALUES (@transactionId, @commandTypeId, @commandId, @commandData)
						
		BEGIN TRY	

			-- event provider
			MERGE dbo.EventProvider AS target
			USING (VALUES (@eventProviderId, @aggregateRootId, @aggregateRootTypeId, @transactionId)) AS source (EventProviderId, AggregateRootId, AggregateRootTypeId, TransactionId)
			   ON target.EventProviderId = source.EventProviderId
			WHEN MATCHED THEN
				UPDATE SET target.LatestTransactionId = source.TransactionId
			WHEN NOT MATCHED THEN
				INSERT (EventProviderId, AggregateRootId, AggregateRootTypeId, LatestTransactionId)
				VALUES (source.EventProviderId, source.AggregateRootId, source.AggregateRootTypeId, source.TransactionId);
				
			-- create revision
			INSERT INTO dbo.[EventProviderRevision] ([EventProviderRevisionId], TransactionId, EventProviderId, EventProviderVersion) 
			SELECT DISTINCT e.EventProviderRevisionId, @transactionId, @eventProviderId, e.EventProviderVersion
				FROM @events e

		END TRY
		BEGIN CATCH		
			SET @errorNum = ERROR_NUMBER()

			IF @errorNum IN (2627,2601)
				THROW 51000, 'Concurrency exception with event provider.', 1;	
			ELSE
				THROW
			
		END CATCH

		-- insert event provider events
		INSERT INTO dbo.EventProviderEvent ([EventProviderRevisionId], [EventProviderEventTypeId], [EventProviderEventId], [Sequence], [Data])
		SELECT e.EventProviderRevisionId, e.TypeId, e.EventId, e.[Sequence], e.[Data]
		  FROM @events e		  
		  
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