CREATE PROCEDURE [dbo].[GetDomainEvents]
	@eventProviderId UNIQUEIDENTIFIER	
AS	

  SELECT tep.EventProviderVersion
       , te.[Sequence]
	   , tet.FullName
	   , te.[Data]
    FROM dbo.TransactionEventProvider tep
	JOIN dbo.EventProvider ep ON ep.EventProviderId = tep.EventProviderId
	JOIN dbo.TransactionEvent te ON te.TransactionEventProviderId = tep.TransactionEventProviderId
	JOIN dbo.TransactionEventType tet ON te.TransactionEventTypeId = tet.TransactionEventTypeId
   WHERE ep.EventProviderGuid = @eventProviderId

RETURN 0
