CREATE TABLE [dbo].[EventProviderType]
(
	[EventProviderTypeId] BINARY(16) NOT NULL PRIMARY KEY NONCLUSTERED, 
    [FullName] VARCHAR(512) NOT NULL,
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_EventProviderType] ON [dbo].[EventProviderType] ([FullName])
