CREATE TABLE [dbo].[EventProviderSnapshot]
(	
	[TransactionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[EventProviderSnapshotTypeId] BINARY(16) NOT NULL,
	[Data] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_EventProviderSnapshot_TransactionEventProvider] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transaction]([TransactionId]), 
    CONSTRAINT [FK_EventProviderSnapshot_EventProviderSnapshotType] FOREIGN KEY ([EventProviderSnapshotTypeId]) REFERENCES [dbo].[EventProviderSnapshotType]([EventProviderSnapshotTypeId]),
)
