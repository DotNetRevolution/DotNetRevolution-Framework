﻿CREATE PROCEDURE [dbo].[CreateTransaction]
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
	, @eventProviderSnapshot		dbo.udttEventProviderSnapshot READONLY
	, @events						dbo.udttEvent READONLY	    
AS
BEGIN

	DECLARE @errText VARCHAR(MAX)
		  , @tempEventProviderGuid UNIQUEIDENTIFIER
		  , @newEventProvider BIT
				
	BEGIN TRAN
	
	BEGIN TRY

		-- make sure only one snapshot was given
		IF (SELECT COUNT(1) FROM @eventProviderSnapshot) > 1
		BEGIN
			; THROW 51000, 'Only one snapshot allowed.', 1;			
		END

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
		SET @tempEventProviderGuid = (SELECT EventProviderGuid
										FROM dbo.EventProvider
									   WHERE EventProviderId = @eventProviderId
										 AND EventProviderTypeId = @eventProviderTypeId)
		IF @tempEventProviderGuid IS NULL						
		BEGIN
			SET @newEventProvider = 1

			INSERT INTO dbo.EventProvider (EventProviderGuid, EventProviderId, EventProviderTypeId)		
			VALUES (@eventProviderGuid, @eventProviderId, @eventProviderTypeId)	
		END
		ELSE
		BEGIN
			SET @newEventProvider = 0
			SET @eventProviderGuid = @tempEventProviderGuid
		END
		
		-- create transaction
		INSERT INTO dbo.[Transaction] (TransactionId, EventProviderGuid, EventProviderVersion, [User])
		VALUES (@transactionId, @eventProviderGuid, @eventProviderVersion, @user)

		-- insert command
		INSERT INTO dbo.TransactionCommand (TransactionId, TransactionCommandTypeId, TransactionCommandId, [Data])
		VALUES (@transactionId, @commandTypeId, @commandId, @commandData)
		
		-- event provider descriptor
		IF @newEventProvider = 1
		BEGIN
			-- new event provider descriptor
			INSERT INTO dbo.EventProviderDescriptor (TransactionId, Descriptor)		
			SELECT @transactionId, @eventProviderDescriptor
		END
		ELSE
		BEGIN
			-- insert event provider descriptor if changed
			INSERT INTO dbo.EventProviderDescriptor (TransactionId, Descriptor)		
			SELECT @transactionId, @eventProviderDescriptor
			  FROM (SELECT TOP 1 epd.TransactionId, epd.Descriptor
					  FROM dbo.EventProviderDescriptor epd
					  JOIN dbo.[Transaction] t ON epd.TransactionId = t.TransactionId
					 WHERE t.EventProviderGuid = @eventProviderGuid
				  ORDER BY t.EventProviderVersion DESC) as a
			 WHERE a.Descriptor <> @eventProviderDescriptor		
		END
		 
		-- snapshot provided?
		IF (SELECT COUNT(1) FROM @eventProviderSnapshot) = 1
		BEGIN
				
			-- snapshot type
			IF NOT EXISTS (SELECT EventProviderSnapshotTypeId 
							 FROM dbo.EventProviderSnapshotType 
							WHERE EventProviderSnapshotTypeId = (SELECT TypeId FROM @eventProviderSnapshot))
			BEGIN
				INSERT INTO dbo.EventProviderSnapshotType (EventProviderSnapshotTypeId, FullName)
				SELECT TypeId, TypeFullName
				  FROM @eventProviderSnapshot
			END
		   	
			-- insert snapshot
			INSERT INTO dbo.EventProviderSnapshot (TransactionId, EventProviderSnapshotTypeId, [Data])					
			SELECT @transactionId, eps.TypeId, eps.[Data]
			  FROM @eventProviderSnapshot eps		  
		END

		-- insert transaction events
		INSERT INTO dbo.TransactionEvent (TransactionId, TransactionEventTypeId, TransactionEventId, [Sequence], [Data])
		SELECT @transactionId, e.TypeId, e.EventId, e.[Sequence], e.[Data]
		  FROM @events e
		  
		-- update event provider with latest information
		--UPDATE dbo.EventProvider SET
		--	   LatestTransactionId = @transactionId	
		--	 , CurrentSnapshotId = CASE WHEN (SELECT COUNT(1) FROM @eventProviderSnapshot) = 1 THEN @transactionId ELSE CurrentSnapshotId END
		--	 , CurrentDescriptorId = CASE WHEN @descriptorRowsInserted > 0 THEN @transactionId ELSE CurrentDescriptorId  END		  
		-- WHERE EventProviderGuid = @eventProviderGuid

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
