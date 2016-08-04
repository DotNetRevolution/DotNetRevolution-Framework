CREATE TABLE [dbo].[EventProviderSnapshot]
(	
	[EventProviderTransactionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[EventProviderSnapshotTypeId] BINARY(16) NOT NULL,
	[Data] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_EventProviderSnapshot_EventProviderTransaction] FOREIGN KEY ([EventProviderTransactionId]) REFERENCES [dbo].[EventProviderTransaction]([EventProviderTransactionId]), 
    CONSTRAINT [FK_EventProviderSnapshot_EventProviderSnapshotType] FOREIGN KEY ([EventProviderSnapshotTypeId]) REFERENCES [dbo].[EventProviderSnapshotType]([EventProviderSnapshotTypeId]),
)
