CREATE PROCEDURE [dbo].[SetProjectionPosition]
	  @projectionId				UNIQUEIDENTIFIER
	, @projectionTypeId			BINARY(16)
    , @projectionTypeFullName	VARCHAR(512)	
	, @eventProviderId			UNIQUEIDENTIFIER
	, @eventProviderVersion		INT
AS
	BEGIN TRAN 
		
	BEGIN TRY
				
		-- projection type
		INSERT INTO dbo.ProjectionType (ProjectionTypeId, FullName)
		SELECT @projectionTypeId, @projectionTypeFullName
		 WHERE NOT EXISTS (SELECT ProjectionTypeId
							 FROM dbo.ProjectionType
					 		WHERE FullName = @projectionTypeFullName)

		-- projection position
		MERGE dbo.Projection AS target
		USING (VALUES 
			(@projectionId, @projectionTypeId, @eventProviderId, @eventProviderVersion)
		) AS source (ProjectionId, ProjectionTypeId, EventProviderId, EventProviderVersion)
			ON  target.ProjectionTypeId = source.ProjectionTypeId
			AND target.ProjectionId = source.ProjectionId
			AND target.EventProviderId = source.EventProviderId			
		WHEN MATCHED THEN
			UPDATE SET [EventProviderVersion] = source.EventProviderVersion
		WHEN NOT MATCHED BY TARGET THEN
			INSERT (ProjectionId, ProjectionTypeId, EventProviderId, EventProviderVersion)
			VALUES (source.ProjectionId, source.ProjectionTypeId, source.EventProviderId, source.EventProviderVersion)
		;
			
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

GO