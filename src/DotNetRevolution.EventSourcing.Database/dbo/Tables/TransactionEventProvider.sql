CREATE TABLE [dbo].[TransactionEventProvider]
(
	[TransactionEventProviderId] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[TransactionId] BIGINT NOT NULL,
	[EventProviderId] INT NOT NULL,	
	[EventProviderVersion] INT NOT NULL, 
    CONSTRAINT [FK_TransactionEventProvider_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transaction]([TransactionId]), 
    CONSTRAINT [FK_TransactionEventProvider_EventProvider] FOREIGN KEY ([EventProviderId]) REFERENCES [dbo].[EventProvider]([EventProviderId]), 
    CONSTRAINT [AK_TransactionEventProvider_TransactionId_EventProviderId] UNIQUE ([TransactionId],[EventProviderId]), 
    CONSTRAINT [AK_TransactionEventProvider_EventProviderId_EventProviderVersion] UNIQUE ([EventProviderId],[EventProviderVersion])
)
