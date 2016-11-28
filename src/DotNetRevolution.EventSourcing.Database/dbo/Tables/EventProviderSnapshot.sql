CREATE TABLE [dbo].[EventProviderSnapshot]
(	
	[EventProviderRevisionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[EventProviderSnapshotTypeId] BINARY(16) NOT NULL,
	[Data] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_EventProviderSnapshot_EventProviderTransaction] FOREIGN KEY ([EventProviderRevisionId]) REFERENCES [dbo].[EventProviderRevision]([EventProviderRevisionId]), 
    CONSTRAINT [FK_EventProviderSnapshot_EventProviderSnapshotType] FOREIGN KEY ([EventProviderSnapshotTypeId]) REFERENCES [dbo].[EventProviderSnapshotType]([EventProviderSnapshotTypeId]),
)
