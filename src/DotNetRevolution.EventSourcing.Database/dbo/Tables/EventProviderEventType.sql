CREATE TABLE [dbo].[EventProviderEventType]
(
	[EventProviderEventTypeId] BINARY(16) NOT NULL PRIMARY KEY NONCLUSTERED, 
    [FullName] VARCHAR(512) NOT NULL
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_EventProviderEventType] ON [dbo].[EventProviderEventType] ([FullName])
