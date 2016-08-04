CREATE TABLE [dbo].[TransactionAnnouncement]
(
    [TransactionAnnouncementId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED,
    [EventProviderTransactionId] UNIQUEIDENTIFIER NOT NULL, 
	[Announced] DATETIME2 NOT NULL, 
    CONSTRAINT [FK_TransactionAnnouncement_Transaction] FOREIGN KEY ([EventProviderTransactionId]) REFERENCES [dbo].[EventProviderTransaction]([EventProviderTransactionId]), 
)
GO

CREATE CLUSTERED INDEX [IX_TransactionAnnouncement] ON [dbo].[TransactionAnnouncement] ([EventProviderTransactionId])
