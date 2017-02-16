CREATE PROCEDURE [dbo].[GetTransactionsByAggregateRootType]
	  @aggregateRootTypeId VARBINARY(16)
	, @skip INT
	, @take INT	
AS	
	SET NOCOUNT ON;

	DECLARE @revisions AS TABLE (EventProviderId UNIQUEIDENTIFIER, AggregateRootId UNIQUEIDENTIFIER, EventProviderRevisionId UNIQUEIDENTIFIER, TransactionId UNIQUEIDENTIFIER, EventProviderVersion INT)

	; WITH cte_page AS (

		SELECT EventProviderId, AggregateRootId 
		  FROM [dbo].[EventProvider] ep
		 WHERE ep.AggregateRootTypeId = @aggregateRootTypeId
	  ORDER BY EventProviderId
		OFFSET @skip ROWS
	FETCH NEXT @take ROWS ONLY 

	)

	 -- load which revisions to fetch
	 INSERT INTO @revisions (EventProviderId, AggregateRootId, EventProviderRevisionId, TransactionId, EventProviderVersion)
	 SELECT epr.EventProviderId, ep.AggregateRootId, EventProviderRevisionId, TransactionId, EventProviderVersion
	   FROM [dbo].[EventProviderRevision] epr
	   JOIN dbo.EventProvider ep ON epr.EventProviderId = ep.EventProviderId	  
	   JOIN cte_page p ON epr.EventProviderId = p.EventProviderId	  

	 -- get revisions
	 SELECT r.EventProviderId, r.AggregateRootId
		  , t.EventProviderDescriptor 
	      , r.EventProviderRevisionId, r.EventProviderVersion, r.TransactionId		  
		  , t.Metadata
		  , tc.TransactionCommandTypeId, tc.[Data]
	   FROM @revisions r
	   JOIN [dbo].[Transaction] t ON r.TransactionId = t.TransactionId
       JOIN [dbo].[TransactionCommand] tc ON r.TransactionId = tc.TransactionId
	   
	-- get events
	SELECT r.EventProviderId, r.EventProviderRevisionId
		 , epe.[Sequence], epe.EventProviderEventTypeId, epe.[Data] 
	  FROM @revisions r
	  JOIN [dbo].[EventProviderEvent] epe ON r.EventProviderRevisionId = epe.EventProviderRevisionId

RETURN 0
