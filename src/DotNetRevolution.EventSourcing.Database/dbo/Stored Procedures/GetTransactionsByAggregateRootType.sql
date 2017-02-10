CREATE PROCEDURE [dbo].[GetTransactionsByAggregateRootType]
	  @aggregateRootTypeId VARBINARY(16)
	, @skip INT
	, @take INT	
AS	
	DECLARE @eventProviders AS TABLE (EventProviderId UNIQUEIDENTIFIER, AggregateRootId UNIQUEIDENTIFIER)
	DECLARE @revisions AS TABLE (EventProviderId UNIQUEIDENTIFIER, EventProviderRevisionId UNIQUEIDENTIFIER, TransactionId UNIQUEIDENTIFIER, EventProviderVersion INT)

	-- load which event providers to fetch
	INSERT INTO @eventProviders (EventProviderId, AggregateRootId)
	SELECT EventProviderId, AggregateRootId
	  FROM [dbo].[EventProvider] ep
	 WHERE ep.AggregateRootTypeId = @aggregateRootTypeId
	 ORDER BY EventProviderId
	OFFSET @skip ROWS
	 FETCH NEXT @take ROWS ONLY

	 -- load which revisions to fetch
	 INSERT INTO @revisions (EventProviderId, EventProviderRevisionId, TransactionId, EventProviderVersion)
	 SELECT epr.EventProviderId, EventProviderRevisionId, TransactionId, EventProviderVersion
	   FROM [dbo].[EventProviderRevision] epr
	   JOIN @eventProviders ep ON epr.EventProviderId = ep.EventProviderId	  

	 -- get revisions
	 SELECT ep.EventProviderId, ep.AggregateRootId
		  , epd.Descriptor 
	      , r.EventProviderRevisionId, r.EventProviderVersion, r.TransactionId		  
		  , t.Metadata
		  , tc.TransactionCommandTypeId, tc.[Data]
	   FROM @eventProviders ep 
	   JOIN [dbo].[EventProviderDescriptor] epd ON ep.EventProviderId = epd.EventProviderId
	   JOIN @revisions r ON ep.EventProviderId = r.EventProviderId
	   JOIN [dbo].[Transaction] t ON r.TransactionId = t.TransactionId
       JOIN [dbo].[TransactionCommand] tc ON r.TransactionId = tc.TransactionId
	   
	-- get events
	SELECT r.EventProviderRevisionId
		 , epe.[Sequence], epe.EventProviderEventTypeId, epe.[Data] 
	  FROM @revisions r
	  JOIN [dbo].[EventProviderEvent] epe ON r.EventProviderRevisionId = epe.EventProviderRevisionId
  ORDER BY r.EventProviderRevisionId, r.EventProviderVersion, epe.[Sequence]

RETURN 0
