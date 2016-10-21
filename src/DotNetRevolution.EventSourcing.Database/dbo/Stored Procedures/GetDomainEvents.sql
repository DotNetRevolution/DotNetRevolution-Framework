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
		  
    DECLARE @transactionTable TABLE 
		  (			
			  TransactionId UNIQUEIDENTIFIER NOT NULL
			, EventProviderVersion INT NOT NULL
		  )
	
	-- get event provider table id
	SET @eventProviderId = (SELECT ep.EventProviderId
								FROM dbo.EventProvider ep
							   WHERE ep.AggregateRootId = @aggregateRootId
								 AND ep.AggregateRootTypeId = @aggregateRootTypeId)

	-- get transactions for event provider
	INSERT INTO @transactionTable (TransactionId, EventProviderVersion)
	SELECT t.EventProviderTransactionId
		 , t.EventProviderVersion
	  FROM dbo.[EventProviderTransaction] t
	 WHERE t.EventProviderId = @eventProviderId
	 
	-- get snapshot
	SELECT TOP 1 
			   @snapshotId = s.EventProviderTransactionId
			 , @snapshotTypeId = s.EventProviderSnapshotTypeId
			 , @snapshotData = s.[Data]
			 , @snapshotVersion = t.EventProviderVersion
		  FROM dbo.EventProviderSnapshot s
		  JOIN @transactionTable t ON s.EventProviderTransactionId = t.TransactionId
		ORDER BY t.EventProviderVersion DESC

	-- select event provider data
	SELECT @eventProviderId 'eventProviderId'
		 , @snapshotVersion 'snapshotVersion'
		 , @snapshotTypeId 'snapshotTypeId'
	     , @snapshotData 'snapshotData'	  

	-- select events
	SELECT t.EventProviderVersion
		 , te.[Sequence]
		 , te.TransactionEventTypeId
		 , te.[Data]		 
	  FROM @transactionTable t
	  JOIN dbo.TransactionEvent te ON te.EventProviderTransactionId = t.TransactionId
     WHERE @snapshotId IS NULL 
	    OR t.EventProviderVersion > @snapshotVersion
	   
RETURN 0
