USE [DotNetRevolution.EventSourcing.Database]
GO

DECLARE	@return_value Int,
		@eventProviders udtteventprovider,
		@events udttevent,
		@commandData varbinary(max),
		@eventProviderGuid uniqueidentifier,
		@commandId uniqueidentifier

set @eventProviderGuid = NEWID()
set @commandId = NEWID()
select @commandData = CONVERT(VARBINARY(MAX), 'CommandData')

insert into @events (EventProviderGuid, EventGuid, Type, Sequence, Data)
SELECT @eventProviderGuid, NEWID(), 'TestEvent', 0, CONVERT(VARBINARY(MAX), 'EventData')

insert into @eventProviders (EventProviderGuid, Type, Version, Descriptor)
SELECT @eventProviderGuid, 'TestProvider', 0, 'Test Provider Desc1'

EXEC	@return_value = [dbo].[CreateTransaction]
		@user = N'Test',
		@commandGuid = @commandId,
		@commandType = 'Test',
		@commandData = @commandData,
		@eventProviders = @eventProviders,
		@events = @events

SELECT	@return_value as 'Return Value'

GO

DECLARE	@return_value Int,
		@eventProviders udtteventprovider,
		@events udttevent,
		@commandData varbinary(max),
		@eventProviderGuid uniqueidentifier,
		@commandId uniqueidentifier

set @eventProviderGuid = '7a1900ee-b1ed-4fd3-9d6e-de681a1802aa'
set @commandId = NEWID()
select @commandData = CONVERT(VARBINARY(MAX), 'CommandData')

insert into @events (EventProviderGuid, EventGuid, Type, Sequence, Data)
SELECT @eventProviderGuid, NEWID(), 'TestEvent', 0, CONVERT(VARBINARY(MAX), 'EventData')

insert into @eventProviders (EventProviderGuid, Type, Version, Descriptor)
SELECT @eventProviderGuid, 'TestProvider', 0, 'Test Provider Desc'

SELECT 1, ep.EventProviderId, eps.Descriptor
FROM @eventProviders eps
JOIN dbo.EventProviderType ept ON ept.FullName = eps.[Type]
JOIN dbo.EventProvider ep ON  eps.EventProviderGuid = ep.EventProviderGuid
						AND ept.EventProviderTypeId = ep.EventProviderTypeId
left JOIN (SELECT ROW_NUMBER() OVER (PARTITION BY epd.EventProviderId ORDER BY t.Processed DESC) 'row_num'
, epd.Descriptor
, epd.EventProviderId
		FROM dbo.[EventProviderDescriptor] epd
		JOIN dbo.[Transaction] t ON epd.TransactionId = t.TransactionId) descriptors ON ep.EventProviderId = descriptors.EventProviderId
		AND (descriptors.row_num IS NULL
		 OR (descriptors.row_num = 1 and eps.Descriptor <> descriptors.Descriptor))
		 