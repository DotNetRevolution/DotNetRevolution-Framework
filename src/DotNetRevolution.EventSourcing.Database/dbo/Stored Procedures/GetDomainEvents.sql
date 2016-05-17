CREATE PROCEDURE [dbo].[GetDomainEvents]
	  @eventProviderId UNIQUEIDENTIFIER	
	, @eventProviderType VARCHAR(512)
AS	
  IF OBJECT_ID('tempdb..#events') IS NOT NULL DROP TABLE #events

  -- load events in temp table
  SELECT tep.EventProviderVersion
	   , epd.Descriptor
       , te.[Sequence]
	   , tet.FullName
	   , te.[Data]
    INTO #events
    FROM dbo.TransactionEventProvider tep	
	JOIN dbo.EventProvider ep ON ep.EventProviderId = tep.EventProviderId
	join dbo.EventProviderDescriptor epd ON epd.EventProviderId = ep.EventProviderId
	JOIN dbo.EventProviderType ept ON ep.EventProviderTypeId = ep.EventProviderTypeId
	JOIN dbo.TransactionEvent te ON te.TransactionEventProviderId = tep.TransactionEventProviderId
	JOIN dbo.TransactionEventType tet ON te.TransactionEventTypeId = tet.TransactionEventTypeId
   WHERE ep.EventProviderGuid = @eventProviderId
     AND ept.FullName = @eventProviderType

  -- select latest descriptor
  SELECT TOP(1) e.Descriptor
    FROM #events e
ORDER BY e.EventProviderVersion DESC

  -- select events
  SELECT e.EventProviderVersion
       , e.[Sequence]
	   , e.FullName
	   , e.[Data]
	FROM #events e

  DROP TABLE #events

RETURN 0
