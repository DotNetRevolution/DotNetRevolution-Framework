CREATE TABLE [dbo].[EventProviderSnapshot]
(	
	[TransactionEventProviderId] BIGINT NOT NULL PRIMARY KEY,
	[EventProviderSnapshotTypeId] INT NOT NULL,
	[Data] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_EventProviderSnapshot_TransactionEventProvider] FOREIGN KEY ([TransactionEventProviderId]) REFERENCES [dbo].[TransactionEventProvider]([TransactionEventProviderId]),
)
