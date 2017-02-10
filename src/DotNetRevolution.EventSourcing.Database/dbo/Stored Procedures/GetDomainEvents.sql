CREATE PROCEDURE [dbo].[GetDomainEvents]
	  @aggregateRootId UNIQUEIDENTIFIER	
	, @aggregateRootTypeId BINARY(16)
AS	
	SET NOCOUNT ON;

	DECLARE @snapshotId UNIQUEIDENTIFIER
		  , @snapshotTypeId BINARY(16)
		  , @snapshotData VARBINARY(MAX)
		  , @eventProviderId UNIQUEIDENTIFIER
		  , @snapshotVersion INT
		  , @snapshotRevisionId UNIQUEIDENTIFIER
		  
    DECLARE @revisionTable TABLE 
		  (			
			  EventProviderRevisionId UNIQUEIDENTIFIER NOT NULL
			, EventProviderVersion INT NOT NULL
		  )
	
	-- get event provider table id
	SET @eventProviderId = (SELECT ep.EventProviderId
							  FROM dbo.EventProvider ep
							 WHERE ep.AggregateRootTypeId = @aggregateRootTypeId
							   AND ep.AggregateRootId = @aggregateRootId)

	-- get transactions for event provider
	INSERT INTO @revisionTable (EventProviderRevisionId, EventProviderVersion)
	SELECT epr.EventProviderRevisionId
		 , epr.EventProviderVersion
	  FROM dbo.[EventProviderRevision] epr
	 WHERE epr.EventProviderId = @eventProviderId
	 
	-- get snapshot
	SELECT TOP 1 
			   @snapshotId = s.[EventProviderRevisionId]
			 , @snapshotTypeId = s.EventProviderSnapshotTypeId
			 , @snapshotData = s.[Data]
			 , @snapshotVersion = r.EventProviderVersion
			 , @snapshotRevisionId = r.EventProviderRevisionId
		  FROM dbo.EventProviderSnapshot s
		  JOIN @revisionTable r ON s.EventProviderRevisionId = r.EventProviderRevisionId
	  ORDER BY r.EventProviderVersion DESC

	-- select event provider data
	SELECT @eventProviderId 'eventProviderId'
		 , @snapshotVersion 'snapshotVersion'
		 , @snapshotTypeId 'snapshotTypeId'
	     , @snapshotData 'snapshotData'	  
	     , @snapshotRevisionId 'snapshotRevisionId'	  

	-- select events
	SELECT r.EventProviderRevisionId
		 , r.EventProviderVersion
		 , epe.[Sequence]
		 , epe.[EventProviderEventTypeId]
		 , epe.[Data]		 
	  FROM @revisionTable r
	  JOIN dbo.EventProviderEvent epe ON epe.[EventProviderRevisionId] = r.EventProviderRevisionId
     WHERE @snapshotId IS NULL 
	    OR r.EventProviderVersion > @snapshotVersion
	   
RETURN 0
