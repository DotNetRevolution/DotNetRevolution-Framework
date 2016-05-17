CREATE PROCEDURE [dbo].[CreateTransaction]
	  @user						NVARCHAR(256)
	, @commandGuid				UNIQUEIDENTIFIER
    , @commandType				VARCHAR (512)
    , @commandData		        VARBINARY (MAX)
	, @eventProviders			udttEventProvider READONLY   	
	, @events					udttEvent READONLY	    
AS
BEGIN

	BEGIN TRAN

	BEGIN TRY
		
		DECLARE @transactionId BIGINT
			  , @commandTypeId INT

		-- insert new event provider types
		INSERT INTO dbo.EventProviderType (FullName)
		SELECT [Type] 
		  FROM @eventProviders ep
		 WHERE NOT EXISTS(SELECT EventProviderTypeId
						    FROM dbo.EventProviderType ept
						   WHERE ept.FullName = ep.[Type])

		-- insert new event providers
		INSERT INTO dbo.EventProvider (EventProviderTypeId, EventProviderGuid)
		SELECT EventProviderTypeId, [EventProviderGuid]
		  FROM @eventProviders eps		  
		  JOIN dbo.EventProviderType ept ON ept.FullName = eps.[Type]
		 WHERE NOT EXISTS(SELECT EventProviderId
						    FROM dbo.EventProvider ep
						   WHERE ep.EventProviderGuid = eps.[EventProviderGuid] 
						     AND ep.EventProviderTypeId = ept.EventProviderTypeId)
		
		-- create transaction
		INSERT INTO dbo.[Transaction] ([User])
		VALUES (@user)

		SET @transactionId = SCOPE_IDENTITY()
		
		-- insert event provider descriptors if changed from last transaction
		INSERT INTO dbo.EventProviderDescriptor (TransactionId, EventProviderId, Descriptor)
		SELECT @transactionId, ep.EventProviderId, eps.Descriptor
		  FROM @eventProviders eps
		  JOIN dbo.EventProviderType ept ON ept.FullName = eps.[Type]
		  JOIN dbo.EventProvider ep ON  eps.EventProviderGuid = ep.EventProviderGuid
									AND ept.EventProviderTypeId = ep.EventProviderTypeId
	 LEFT JOIN (SELECT ROW_NUMBER() OVER (PARTITION BY epd.EventProviderId ORDER BY t.Processed DESC) 'row_num'
					 , epd.Descriptor
					 , epd.EventProviderId
				 FROM dbo.[EventProviderDescriptor] epd
				 JOIN dbo.[Transaction] t ON epd.TransactionId = t.TransactionId) descriptors ON ep.EventProviderId = descriptors.EventProviderId
		 WHERE descriptors.row_num IS NULL
			OR (descriptors.row_num = 1 AND eps.Descriptor <> descriptors.Descriptor)		 
		 
		-- insert if command type does not exist		
		SET @commandTypeId = (SELECT TransactionCommandTypeId 
								FROM dbo.TransactionCommandType
							   WHERE FullName = @commandType)

		IF (@commandTypeId IS NULL)
		BEGIN
			INSERT INTO dbo.TransactionCommandType (FullName) 
			VALUES (@commandType)
	
			SET @commandTypeId = SCOPE_IDENTITY()
		END
		
		-- insert command
		INSERT INTO dbo.TransactionCommand (TransactionId, TransactionCommandTypeId, TransactionCommandGuid, [Data])
		VALUES (@transactionId, @commandTypeId, @commandGuid, @commandData)
		
		-- insert transaction event providers
		INSERT INTO dbo.TransactionEventProvider (TransactionId, EventProviderId, EventProviderVersion)
		SELECT @transactionId, ep.EventProviderId, eps.[Version]
		  FROM @eventProviders eps
		  JOIN dbo.EventProviderType ept ON eps.[Type] = ept.FullName
		  JOIN dbo.EventProvider ep ON  eps.EventProviderGuid = ep.EventProviderGuid
									AND ept.EventProviderTypeId = ep.EventProviderTypeId
		
		-- insert transaction event types  
		INSERT INTO dbo.TransactionEventType (FullName)
		SELECT [Type] 
		  FROM @events e
		 WHERE NOT EXISTS(SELECT TransactionEventTypeId
						    FROM dbo.TransactionEventType tet
						   WHERE tet.FullName = e.[Type])
		
		-- insert transaction events
		INSERT INTO dbo.TransactionEvent (TransactionEventProviderId, TransactionEventTypeId, TransactionEventGuid, [Sequence], [Data])
		SELECT tep.TransactionEventProviderId, tet.TransactionEventTypeId, e.[EventGuid], e.[Sequence], e.[Data]
		  FROM @events e
		  JOIN dbo.TransactionEventType tet ON e.[Type] = tet.FullName
		  JOIN @eventProviders eps ON e.EventProviderGuid = eps.EventProviderGuid
		  JOIN dbo.EventProviderType ept ON eps.[Type] = ept.FullName
		  JOIN dbo.EventProvider ep ON  e.EventProviderGuid = ep.EventProviderGuid
									AND ept.EventProviderTypeId = ep.EventProviderTypeId
		  JOIN dbo.TransactionEventProvider tep ON ep.EventProviderId = tep.EventProviderId												
		 WHERE tep.TransactionId = @transactionId

		-- nothing failed
		COMMIT TRAN

		-- return success to caller
		RETURN 0
	END TRY

	BEGIN CATCH
		-- something failed
		ROLLBACK TRAN
		
		-- print error
		PRINT ERROR_MESSAGE() 

		-- return failure to caller
		RETURN 1
		
	END CATCH
END
