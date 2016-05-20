CREATE PROCEDURE [dbo].[GetDomainEvents]
	  @eventProviderId UNIQUEIDENTIFIER	
	, @eventProviderType VARCHAR(512)
AS	
	IF OBJECT_ID('tempdb..#events') IS NOT NULL DROP TABLE #events

	DECLARE @snapshotId INT
		  , @snapshotFullName VARCHAR(512)
		  , @snapshotData VARBINARY(MAX)

	-- find latest snapshot
	SELECT TOP (1) 
		   @snapshotId = eps.TransactionEventProviderId
		 , @snapshotData = eps.[Data]
		 , @snapshotFullName = epst.FullName
	  FROM dbo.EventProviderSnapshot eps
	  JOIN dbo.EventProviderSnapshotType epst ON eps.EventProviderSnapshotTypeId = epst.EventProviderSnapshotTypeId
	  JOIN dbo.TransactionEventProvider tep ON eps.TransactionEventProviderId = tep.TransactionEventProviderId
	  JOIN dbo.EventProvider ep ON tep.EventProviderId = ep.EventProviderId
	  JOIN dbo.EventProviderType ept ON ep.EventProviderTypeId = ept.EventProviderTypeId
	 WHERE ep.EventProviderGuid = @eventProviderId
       AND ept.FullName = @eventProviderType
  ORDER BY tep.EventProviderVersion DESC

    -- load events in temp table
	SELECT tep.EventProviderVersion
	 	 , epd.Descriptor
		 , te.[Sequence]
		 , tet.FullName
		 , te.[Data]
	  INTO #events
	  FROM dbo.TransactionEventProvider tep	
	  JOIN dbo.EventProvider ep ON ep.EventProviderId = tep.EventProviderId
	  JOIN dbo.EventProviderDescriptor epd ON epd.EventProviderId = ep.EventProviderId
	  JOIN dbo.EventProviderType ept ON ep.EventProviderTypeId = ep.EventProviderTypeId
	  JOIN dbo.TransactionEvent te ON te.TransactionEventProviderId = tep.TransactionEventProviderId
	  JOIN dbo.TransactionEventType tet ON te.TransactionEventTypeId = tet.TransactionEventTypeId 
     WHERE ep.EventProviderGuid = @eventProviderId
       AND ept.FullName = @eventProviderType
	   AND (@snapshotId IS NULL OR tep.TransactionEventProviderId > @snapshotId)

	-- select latest descriptor
	SELECT TOP(1) e.Descriptor
	  FROM #events e
  ORDER BY e.EventProviderVersion DESC

	-- select snapshot
    SELECT @snapshotFullName 'snapshotFullName'
	     , @snapshotData 'snapshotData'

	-- select events
	SELECT e.EventProviderVersion
		 , e.[Sequence]
		 , e.FullName
	 	 , e.[Data]
	  FROM #events e

	DROP TABLE #events

RETURN 0
