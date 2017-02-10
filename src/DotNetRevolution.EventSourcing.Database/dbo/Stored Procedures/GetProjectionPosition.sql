CREATE PROCEDURE [dbo].[GetProjectionPosition]
	  @projectionId				UNIQUEIDENTIFIER
	, @projectionTypeId			BINARY(16)
	, @eventProviderId			UNIQUEIDENTIFIER
AS

	SELECT EventProviderVersion 
	  FROM dbo.Projection p
	 WHERE p.ProjectionTypeId = @projectionTypeId
	   AND p.ProjectionId = @projectionId
	   AND p.EventProviderId = @eventProviderId	   
	   	   
RETURN 0
