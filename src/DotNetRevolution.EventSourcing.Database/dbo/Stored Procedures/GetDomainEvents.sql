CREATE PROCEDURE [dbo].[GetDomainEvents]
	  @eventProviderId UNIQUEIDENTIFIER	
	, @eventProviderType VARCHAR(512)
AS	
	IF OBJECT_ID('tempdb..#events') IS NOT NULL DROP TABLE #events

	DECLARE @snapshotId INT
		  , @snapshotFullName VARCHAR(512)
		  , @snapshotData VARBINARY(MAX)
		  , @eventProviderTableId INT
		  , @eventProviderVersion INT
	
	-- get event provider table id
	SELECT @eventProviderTableId = [dbo].[GetEventProviderId](@eventProviderId,@eventProviderType)
	
	-- get snapshot and version
	SELECT @snapshotId = eps.TransactionEventProviderId
		 , @snapshotData = eps.[Data]
		 , @snapshotFullName = epst.FullName
		 , @eventProviderVersion = ep.CurrentVersion
	  FROM dbo.EventProvider ep
 LEFT JOIN dbo.EventProviderSnapshot eps ON ep.LatestSnapshotId = eps.TransactionEventProviderId
 LEFT JOIN dbo.EventProviderSnapshotType epst ON eps.EventProviderSnapshotTypeId = epst.EventProviderSnapshotTypeId
	 WHERE ep.EventProviderId = @eventProviderTableId
	  	  
    -- load events in temp table
	SELECT tep.EventProviderVersion
		 , te.[Sequence]
		 , tet.FullName
		 , te.[Data]
	  INTO #events
	  FROM dbo.TransactionEventProvider tep	
	  JOIN dbo.EventProvider ep ON ep.EventProviderId = tep.EventProviderId 	  
	  JOIN dbo.TransactionEvent te ON te.TransactionEventProviderId = tep.TransactionEventProviderId
	  JOIN dbo.TransactionEventType tet ON te.TransactionEventTypeId = tet.TransactionEventTypeId 
     WHERE ep.EventProviderId = @eventProviderTableId
	   AND (@snapshotId IS NULL OR tep.TransactionEventProviderId > @snapshotId)

	-- select event provider data
	SELECT epd.Descriptor
		 , @eventProviderVersion 'currentVersion'
		 , @snapshotFullName 'snapshotFullName'
	     , @snapshotData 'snapshotData'
	  FROM dbo.EventProvider ep
	  JOIN dbo.EventProviderDescriptor epd ON ep.CurrentEventProviderDescriptorId = epd.TransactionEventProviderId  
	 WHERE ep.EventProviderId = @eventProviderTableId

	-- select events
	SELECT e.EventProviderVersion
		 , e.[Sequence]
		 , e.FullName
	 	 , e.[Data]
	  FROM #events e

	DROP TABLE #events

RETURN 0
