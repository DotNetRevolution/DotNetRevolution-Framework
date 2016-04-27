CREATE TABLE [dbo].[TransactionEvent]
(	
	[TransactionEventId] BIGINT NOT NULL IDENTITY PRIMARY KEY,
	[TransactionEventProviderId] BIGINT NOT NULL,
    [TransactionEventTypeId] INT NOT NULL,
    [TransactionEventGuid] UNIQUEIDENTIFIER NOT NULL,
    [Sequence] INT NOT NULL, 
    [Data] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_Event_TransactionId] FOREIGN KEY ([TransactionEventProviderId]) REFERENCES [dbo].[TransactionEventProvider]([TransactionEventProviderId]), 
    CONSTRAINT [FK_TransactionEvent_TransactionEventType] FOREIGN KEY ([TransactionEventTypeId]) REFERENCES [dbo].[TransactionEventType]([TransactionEventTypeId]), 
    CONSTRAINT [AK_TransactionEvent_TransactionEventGuid] UNIQUE ([TransactionEventGuid])
)

GO
