CREATE FUNCTION [dbo].[GetEventProviderId]
(
	  @eventProviderId UNIQUEIDENTIFIER	
	, @eventProviderType VARCHAR(512)
)
RETURNS INT
AS
BEGIN
	DECLARE @retVal INT

	SELECT @retVal = EventProviderId 
	  FROM dbo.EventProvider ep
	  JOIN dbo.EventProviderType ept ON ep.EventProviderTypeId = ept.EventProviderTypeId
	 WHERE ep.EventProviderGuid = @eventProviderId
       AND ept.FullName = @eventProviderType

	RETURN @retVal
END
