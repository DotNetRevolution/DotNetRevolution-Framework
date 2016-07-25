CREATE TABLE [dbo].[EventProvider]
(	
	[EventProviderGuid] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED,
    [EventProviderTypeId] BINARY(16) NOT NULL, 	
	[EventProviderId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_EventProvider_EventProviderType] FOREIGN KEY ([EventProviderTypeId]) REFERENCES [dbo].[EventProviderType]([EventProviderTypeId])
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_EventProvider] ON [dbo].[EventProvider] ([EventProviderTypeId],[EventProviderId])
