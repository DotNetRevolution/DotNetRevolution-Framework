CREATE TABLE [dbo].[EventProviderEvent]
(	
	[EventProviderEventId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED,
	[EventProviderRevisionId] UNIQUEIDENTIFIER NOT NULL,
    [EventProviderEventTypeId] BINARY(16) NOT NULL,
    [Sequence] INT NOT NULL, 
    [Data] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_TransactionEvent_EventProviderTransactionId] FOREIGN KEY ([EventProviderRevisionId]) REFERENCES [dbo].[EventProviderRevision]([EventProviderRevisionId]), 
    CONSTRAINT [FK_TransactionEvent_EventProviderEventType] FOREIGN KEY ([EventProviderEventTypeId]) REFERENCES [dbo].[EventProviderEventType]([EventProviderEventTypeId]), 
    CONSTRAINT [AK_TransactionEvent_EventProviderTransactionId_Sequence] UNIQUE ([EventProviderRevisionId],[Sequence]),     
)

GO


CREATE CLUSTERED INDEX [IX_EventProviderEvent] ON [dbo].[EventProviderEvent] ([EventProviderRevisionId])
