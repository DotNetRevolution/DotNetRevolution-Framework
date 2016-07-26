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
		  , @eventProviderDescriptor VARCHAR(MAX)
		  
    DECLARE @transactionTable TABLE 
		  (			
			  TransactionId UNIQUEIDENTIFIER
			, EventProviderVersion INT
		  )
	
	-- get event provider table id
	SET @eventProviderGuid = (SELECT ep.EventProviderGuid
								FROM dbo.EventProvider ep
							   WHERE ep.EventProviderId = @eventProviderId
								 AND ep.EventProviderTypeId = @eventProviderTypeId)

	-- get transactions for event provider
	INSERT INTO @transactionTable (TransactionId, EventProviderVersion)
	SELECT t.TransactionId
		 , t.EventProviderVersion
	  FROM dbo.[Transaction] t
	 WHERE t.EventProviderGuid = @eventProviderGuid
	 
	-- get snapshot
	SELECT TOP 1 
			   @snapshotId = s.TransactionId
			 , @snapshotTypeId = s.EventProviderSnapshotTypeId
			 , @snapshotData = s.[Data]
			 , @snapshotVersion = t.EventProviderVersion
		  FROM dbo.EventProviderSnapshot s
		  JOIN @transactionTable t ON s.TransactionId = t.TransactionId
		ORDER BY t.EventProviderVersion DESC

	-- get descriptor
	SELECT TOP 1 
			   @eventProviderDescriptor = d.Descriptor
		  FROM dbo.EventProviderDescriptor d
		  JOIN @transactionTable t ON d.TransactionId = t.TransactionId
		ORDER BY t.EventProviderVersion DESC

	-- select event provider data
	SELECT @eventProviderDescriptor 'descriptor'
		 , MAX(t.EventProviderVersion) 'currentVersion'
		 , @snapshotTypeId 'snapshotTypeId'
	     , @snapshotData 'snapshotData'	  
	  FROM @transactionTable t

	-- select events
	SELECT t.EventProviderVersion
		 , te.[Sequence]
		 , tet.TransactionEventTypeId
		 , te.[Data]
	  FROM @transactionTable t
	  JOIN dbo.TransactionEvent te ON te.TransactionId = t.TransactionId
	  JOIN dbo.TransactionEventType tet ON te.TransactionEventTypeId = tet.TransactionEventTypeId 
     WHERE @snapshotId IS NULL 
	    OR t.EventProviderVersion > @snapshotVersion
	   
RETURN 0
