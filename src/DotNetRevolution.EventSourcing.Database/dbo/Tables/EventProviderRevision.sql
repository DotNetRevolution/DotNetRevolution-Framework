CREATE TABLE [dbo].[EventProviderRevision]
(
	[EventProviderRevisionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED,
	[TransactionId] UNIQUEIDENTIFIER NOT NULL, 	
	[EventProviderId] UNIQUEIDENTIFIER NOT NULL,
	[EventProviderVersion] INT NOT NULL,
 
    CONSTRAINT [AK_EventProviderTransaction_EventProviderId_EventProviderVersion] UNIQUE ([EventProviderId],[EventProviderVersion]), 
    CONSTRAINT [FK_EventProviderRevision_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transaction]([TransactionId])
)

GO

CREATE CLUSTERED INDEX [IX_EventProviderRevision] ON [dbo].[EventProviderRevision]([EventProviderId])

GO
