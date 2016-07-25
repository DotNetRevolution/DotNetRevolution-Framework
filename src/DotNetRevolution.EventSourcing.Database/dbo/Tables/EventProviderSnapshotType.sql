CREATE TABLE [dbo].[EventProviderSnapshotType]
(
	[EventProviderSnapshotTypeId] BINARY(16) NOT NULL PRIMARY KEY NONCLUSTERED, 
    [FullName] VARCHAR(512) NOT NULL
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_EventProviderSnapshotType] ON [dbo].[EventProviderSnapshotType] ([FullName])
