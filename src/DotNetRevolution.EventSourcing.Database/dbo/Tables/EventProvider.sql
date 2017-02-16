CREATE TABLE [dbo].[EventProvider]
(	
	[EventProviderId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED,
    [AggregateRootTypeId] BINARY(16) NOT NULL, 	
	[AggregateRootId] UNIQUEIDENTIFIER NOT NULL,
	[LatestTransactionId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_EventProvider_AggregateRootType] FOREIGN KEY ([AggregateRootTypeId]) REFERENCES [dbo].[AggregateRootType]([AggregateRootTypeId]), 
    CONSTRAINT [FK_EventProvider_Transaction] FOREIGN KEY ([LatestTransactionId]) REFERENCES [dbo].[Transaction]([TransactionId])
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_EventProvider] ON [dbo].[EventProvider] ([AggregateRootTypeId],[AggregateRootId])

GO
