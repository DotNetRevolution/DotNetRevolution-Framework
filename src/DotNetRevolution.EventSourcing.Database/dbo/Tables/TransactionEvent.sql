CREATE TABLE [dbo].[TransactionEvent]
(	
	[TransactionEventId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED,
	[EventProviderTransactionId] UNIQUEIDENTIFIER NOT NULL,
    [TransactionEventTypeId] BINARY(16) NOT NULL,
    [Sequence] INT NOT NULL, 
    [Data] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_TransactionEvent_EventProviderTransactionId] FOREIGN KEY ([EventProviderTransactionId]) REFERENCES [dbo].[EventProviderTransaction]([EventProviderTransactionId]), 
    CONSTRAINT [FK_TransactionEvent_TransactionEventType] FOREIGN KEY ([TransactionEventTypeId]) REFERENCES [dbo].[TransactionEventType]([TransactionEventTypeId]),     
)

GO


CREATE CLUSTERED INDEX [IX_TransactionEvent] ON [dbo].[TransactionEvent] ([EventProviderTransactionId])
