CREATE PROCEDURE [dbo].[GetDomainEvents]
	  @eventProviderId UNIQUEIDENTIFIER	
	, @eventProviderTypeId BINARY(16)
AS	
	SET NOCOUNT ON;

	DECLARE @snapshotId UNIQUEIDENTIFIER
		  , @snapshotTypeId BINARY(16)
		  , @snapshotData VARBINARY(MAX)
		  , @eventProviderGuid UNIQUEIDENTIFIER
		  , @snapshotVersion INT
		  , @snapshotCommitted DATETIME2
		  , @eventProviderDescriptor VARCHAR(MAX)
		  
    DECLARE @transactionTable TABLE 
		  (			
			  TransactionId UNIQUEIDENTIFIER
			, EventProviderVersion INT
			, [Committed] DATETIME2
		  )
	
	-- get event provider table id
	SET @eventProviderGuid = (SELECT ep.EventProviderGuid
								FROM dbo.EventProvider ep
							   WHERE ep.EventProviderId = @eventProviderId
								 AND ep.EventProviderTypeId = @eventProviderTypeId)

	-- get transactions for event provider
	INSERT INTO @transactionTable (TransactionId, EventProviderVersion, [Committed])
	SELECT t.EventProviderTransactionId
		 , t.EventProviderVersion
		 , t.[Committed]
	  FROM dbo.[EventProviderTransaction] t
	 WHERE t.EventProviderGuid = @eventProviderGuid
	 
	-- get snapshot
	SELECT TOP 1 
			   @snapshotId = s.EventProviderTransactionId
			 , @snapshotTypeId = s.EventProviderSnapshotTypeId
			 , @snapshotData = s.[Data]
			 , @snapshotVersion = t.EventProviderVersion
			 , @snapshotCommitted = t.[Committed]
		  FROM dbo.EventProviderSnapshot s
		  JOIN @transactionTable t ON s.EventProviderTransactionId = t.TransactionId
		ORDER BY t.EventProviderVersion DESC

	-- get descriptor
	SELECT TOP 1 
			   @eventProviderDescriptor = d.Descriptor
		  FROM dbo.EventProviderDescriptor d
		  JOIN @transactionTable t ON d.EventProviderTransactionId = t.TransactionId
		ORDER BY t.EventProviderVersion DESC

	-- select event provider data
	SELECT @eventProviderGuid 'guid'
		 , @eventProviderDescriptor 'descriptor'
		 , @snapshotVersion 'snapshotVersion'
		 , @snapshotTypeId 'snapshotTypeId'
	     , @snapshotData 'snapshotData'	  
		 , @snapshotCommitted 'snapshotCommitted'	  

	-- select events
	SELECT t.EventProviderVersion
		 , t.[Committed]
		 , te.[Sequence]
		 , te.TransactionEventTypeId
		 , te.[Data]		 
	  FROM @transactionTable t
	  JOIN dbo.TransactionEvent te ON te.EventProviderTransactionId = t.TransactionId
     WHERE @snapshotId IS NULL 
	    OR t.EventProviderVersion > @snapshotVersion
	   
RETURN 0
