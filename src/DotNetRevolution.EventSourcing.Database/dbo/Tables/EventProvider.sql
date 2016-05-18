CREATE TABLE [dbo].[EventProvider]
(	
	[EventProviderId] INT NOT NULL PRIMARY KEY,
    [EventProviderTypeId] INT NOT NULL, 	
	[EventProviderGuid] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_EventProvider_EventProviderType] FOREIGN KEY ([EventProviderTypeId]) REFERENCES [dbo].[EventProviderType]([EventProviderTypeId]), 
    CONSTRAINT [AK_EventProvider_EventProviderGuid_EventProviderTypeId] UNIQUE ([EventProviderGuid], [EventProviderTypeId]),     	
)

GO
