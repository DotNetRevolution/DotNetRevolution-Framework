﻿CREATE PROCEDURE [dbo].[CreateTransaction]
	  @user						NVARCHAR(256)
	, @commandGuid				UNIQUEIDENTIFIER	
    , @commandTypeFullName		VARCHAR (512)	
    , @commandData		        VARBINARY (MAX)
	, @eventProviders			udttEventProvider READONLY   	
	, @events					udttEvent READONLY	    
AS
BEGIN

	DECLARE @transactionId BIGINT
		  , @commandTypeId INT

	DECLARE @eventProviderTable AS TABLE(
										  TempId INT
										, TableId INT
										, [Guid] UNIQUEIDENTIFIER
										, [TypeId] INT 
										, FullName VARCHAR (512)
										, NewProvider BIT
										, NewType BIT
										, [Version] INT
										)
										
	DECLARE @transactionEventTypeTable AS TABLE(
												  Id INT
												, FullName VARCHAR (512)
												, New BIT
												)

	DECLARE @transactionEventProviders AS TABLE(
												  Id INT
												, [EventProviderId] INT
												)

	INSERT INTO @eventProviderTable (TempId, TableId, [Guid], [TypeId], FullName, [NewProvider], [NewType], [Version])
	SELECT tep.TempId
			, ep.EventProviderId
			, tep.EventProviderGuid
			, ept.EventProviderTypeId
			, tep.TypeFullName
			, CASE WHEN ep.EventProviderId IS NULL THEN 1 ELSE 0 END
			, CASE WHEN ept.EventProviderTypeId IS NULL THEN 1 ELSE 0 END
			, tep.[Version]
		FROM @eventProviders tep
	LEFT JOIN dbo.EventProviderType ept ON tep.TypeFullName = ept.FullName
	LEFT JOIN dbo.EventProvider ep ON  tep.EventProviderGuid = ep.EventProviderGuid
								AND ept.EventProviderTypeId = ep.EventProviderTypeId

	UPDATE @eventProviderTable SET
			TableId = NEXT VALUE FOR dbo.EventProviderSequence
		WHERE NewProvider = 1
		 
	UPDATE @eventProviderTable SET
			TypeId = NEXT VALUE FOR dbo.EventProviderTypeSequence
		WHERE NewType = 1			 

	INSERT INTO @transactionEventTypeTable (Id, FullName, New)
	SELECT DISTINCT tet.TransactionEventTypeId			 
			, e.TypeFullName
			, CASE WHEN tet.TransactionEventTypeId IS NULL THEN 1 ELSE 0 END
		FROM @events e
	LEFT JOIN dbo.TransactionEventType tet ON e.TypeFullName = tet.FullName

	UPDATE @transactionEventTypeTable SET
			Id = NEXT VALUE FOR dbo.TransactionEventTypeSequence
		WHERE New = 1

	SET @commandTypeId = (SELECT TransactionCommandTypeId 
						FROM dbo.TransactionCommandType
						WHERE FullName = @commandTypeFullName)

	BEGIN TRAN
	
	BEGIN TRY
		
		-- insert new event provider types
		INSERT INTO dbo.EventProviderType (EventProviderTypeId, FullName)
		SELECT DISTINCT ep.TypeId, ep.FullName		  
		  FROM @eventProviderTable ep		  
		 WHERE ep.NewType = 1
						   
		-- insert transaction event types  
		INSERT INTO dbo.TransactionEventType (TransactionEventTypeId, FullName)
		SELECT tett.Id, tett.FullName 
		  FROM @transactionEventTypeTable tett
		 WHERE tett.New = 1
		 		   		 
		-- insert if command type does not exist				
		IF (@commandTypeId IS NULL)
		BEGIN
			INSERT INTO dbo.TransactionCommandType (FullName) 
			VALUES (@commandTypeFullName)
	
			SET @commandTypeId = SCOPE_IDENTITY()
		END

		-- insert new event providers
		INSERT INTO dbo.EventProvider (EventProviderId, EventProviderGuid, EventProviderTypeId)		
		SELECT ept.TableId, ept.[Guid], ept.TypeId
		  FROM @eventProviderTable ept
		 WHERE ept.NewProvider = 1
		
		-- create transaction
		INSERT INTO dbo.[Transaction] ([User])
		VALUES (@user)

		SET @transactionId = SCOPE_IDENTITY()
		
		-- insert event provider descriptors if changed from last transaction
		INSERT INTO dbo.EventProviderDescriptor (TransactionId, EventProviderId, Descriptor)
		SELECT @transactionId, ep.TableId, eps.Descriptor
		  FROM @eventProviders eps		  
		  JOIN @eventProviderTable ep ON eps.TempId = ep.TempId
	 LEFT JOIN (SELECT ROW_NUMBER() OVER (PARTITION BY epd.EventProviderId ORDER BY t.Processed DESC) 'row_num'
					 , epd.Descriptor
					 , epd.EventProviderId
				 FROM dbo.[EventProviderDescriptor] epd
				 JOIN dbo.[Transaction] t ON epd.TransactionId = t.TransactionId) descriptors ON ep.TableId = descriptors.EventProviderId
		 WHERE descriptors.row_num IS NULL
			OR (descriptors.row_num = 1 AND eps.Descriptor <> descriptors.Descriptor)		 
		 
		-- insert command
		INSERT INTO dbo.TransactionCommand (TransactionId, TransactionCommandTypeId, TransactionCommandGuid, [Data])
		VALUES (@transactionId, @commandTypeId, @commandGuid, @commandData)
		
		-- insert transaction event providers
		INSERT INTO dbo.TransactionEventProvider (TransactionId, EventProviderId, EventProviderVersion)
		OUTPUT inserted.EventProviderId, inserted.TransactionEventProviderId 
		  INTO @transactionEventProviders (EventProviderId, Id)
		SELECT @transactionId, ept.TableId, ept.[Version]
		  FROM @eventProviderTable ept

		-- insert transaction events
		INSERT INTO dbo.TransactionEvent (TransactionEventProviderId, TransactionEventTypeId, TransactionEventGuid, [Sequence], [Data])
		SELECT tep.Id, tett.Id, e.[EventGuid], e.[Sequence], e.[Data]
		  FROM @events e
		  JOIN @transactionEventTypeTable tett ON tett.FullName = e.TypeFullName		  		  
		  JOIN @eventProviderTable ep ON e.EventProviderTempId = ep.TempId
		  JOIN @transactionEventProviders tep ON ep.TableId = tep.EventProviderId

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
		
		-- print error
		PRINT ERROR_MESSAGE() 

		-- return failure to caller
		RETURN 1
		
	END CATCH
END
