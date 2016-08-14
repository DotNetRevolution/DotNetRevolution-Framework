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
    , @eventProviderVersion			INT             
	, @snapshotTypeId				BINARY(16) = NULL
	, @snapshotTypeFullName			VARCHAR (512) = NULL
	, @snapshotData					VARBINARY (MAX) = NULL
	, @events						dbo.udttEvent READONLY	    
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @errText VARCHAR(MAX)
		  
	DECLARE @insertedEventProviderGuid AS TABLE (id UNIQUEIDENTIFIER)
				
	BEGIN TRAN
	
	BEGIN TRY
	
		-- event provider type
		INSERT INTO dbo.EventProviderType (EventProviderTypeId, FullName)
		SELECT @eventProviderTypeId, @eventProviderTypeFullName
		 WHERE NOT EXISTS (SELECT EventProviderTypeId 
							 FROM dbo.EventProviderType
					 		WHERE EventProviderTypeId = @eventProviderTypeId)

		-- command type					
		INSERT INTO dbo.TransactionCommandType (TransactionCommandTypeId, FullName) 
		SELECT @commandTypeId, @commandTypeFullName
		WHERE NOT EXISTS (SELECT TransactionCommandTypeId 
							FROM dbo.TransactionCommandType
					       WHERE TransactionCommandTypeId = @commandTypeId)
		
		-- insert transaction event types  
		INSERT INTO dbo.TransactionEventType (TransactionEventTypeId, FullName)
		SELECT e.TypeId, e.TypeFullName
		  FROM @events e
	 LEFT JOIN dbo.TransactionEventType tet ON e.TypeId = tet.TransactionEventTypeId		
		 WHERE tet.TransactionEventTypeId IS NULL
		
		-- event provider
		INSERT INTO dbo.EventProvider (EventProviderGuid, EventProviderId, EventProviderTypeId)		
		OUTPUT inserted.EventProviderGuid INTO @insertedEventProviderGuid
		SELECT @eventProviderGuid, @eventProviderId, @eventProviderTypeId
		 WHERE NOT EXISTS (SELECT EventProviderGuid
							 FROM dbo.EventProvider
							WHERE EventProviderGuid = @eventProviderGuid)
									
		-- create transaction
		INSERT INTO dbo.[EventProviderTransaction] (EventProviderTransactionId, EventProviderGuid, EventProviderVersion)
		VALUES (@transactionId, @eventProviderGuid, @eventProviderVersion)

		-- insert transaction information
		INSERT INTO dbo.[TransactionInformation] (EventProviderTransactionId, [User])
		VALUES (@transactionId, @user)

		-- insert command
		INSERT INTO dbo.TransactionCommand (EventProviderTransactionId, TransactionCommandTypeId, TransactionCommandId, [Data])
		VALUES (@transactionId, @commandTypeId, @commandId, @commandData)
		
		-- event provider descriptor
		IF EXISTS (SELECT * FROM @insertedEventProviderGuid)
		BEGIN
			-- new event provider descriptor
			INSERT INTO dbo.EventProviderDescriptor (EventProviderTransactionId, Descriptor)		
			SELECT @transactionId, @eventProviderDescriptor
		END
		ELSE
		BEGIN
			-- insert event provider descriptor if changed
			INSERT INTO dbo.EventProviderDescriptor (EventProviderTransactionId, Descriptor)		
			SELECT @transactionId, @eventProviderDescriptor
			  FROM (SELECT TOP 1 epd.EventProviderTransactionId, epd.Descriptor
					  FROM dbo.EventProviderDescriptor epd
					  JOIN dbo.[EventProviderTransaction] t ON epd.EventProviderTransactionId = t.EventProviderTransactionId
					 WHERE t.EventProviderGuid = @eventProviderGuid
				  ORDER BY t.EventProviderVersion DESC) as a
			 WHERE a.Descriptor <> @eventProviderDescriptor		
		END
		 
		-- snapshot provided?
		IF (@snapshotTypeId IS NOT NULL AND
		    @snapshotTypeFullName IS NOT NULL AND
			@snapshotData IS NOT NULL)
		BEGIN				
			-- snapshot type
			IF NOT EXISTS (SELECT EventProviderSnapshotTypeId 
							 FROM dbo.EventProviderSnapshotType 
							WHERE EventProviderSnapshotTypeId = @snapshotTypeId)
			BEGIN
				INSERT INTO dbo.EventProviderSnapshotType (EventProviderSnapshotTypeId, FullName)
				VALUES (@snapshotTypeId, @snapshotTypeFullName)
			END
		   	
			-- insert snapshot
			INSERT INTO dbo.EventProviderSnapshot (EventProviderTransactionId, EventProviderSnapshotTypeId, [Data])					
			VALUES (@transactionId, @snapshotTypeId, @snapshotData)
		END

		-- insert transaction events
		INSERT INTO dbo.TransactionEvent (EventProviderTransactionId, TransactionEventTypeId, TransactionEventId, [Sequence], [Data])
		SELECT @transactionId, e.TypeId, e.EventId, e.[Sequence], e.[Data]
		  FROM @events e
		  
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
		ROLLBACK TRAN

		SET @errText = ERROR_MESSAGE()

		RAISERROR(@errText, 16, 1)
		
		-- return failure to caller
		RETURN 1
		
	END CATCH
END
