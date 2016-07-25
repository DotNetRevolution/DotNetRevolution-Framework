CREATE TABLE [dbo].[TransactionEvent]
(	
	[TransactionEventId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED,
	[TransactionId] UNIQUEIDENTIFIER NOT NULL,
    [TransactionEventTypeId] BINARY(16) NOT NULL,
    [Sequence] INT NOT NULL, 
    [Data] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_TransactionEvent_TransactionId] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transaction]([TransactionId]), 
    CONSTRAINT [FK_TransactionEvent_TransactionEventType] FOREIGN KEY ([TransactionEventTypeId]) REFERENCES [dbo].[TransactionEventType]([TransactionEventTypeId]),     
)

GO


CREATE CLUSTERED INDEX [IX_TransactionEvent] ON [dbo].[TransactionEvent] ([TransactionId])
