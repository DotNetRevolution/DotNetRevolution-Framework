CREATE TABLE [dbo].[TransactionAnnouncement]
(
    [TransactionAnnouncementId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED,
    [TransactionId] UNIQUEIDENTIFIER NOT NULL, 
	[Announced] DATETIME2 NOT NULL, 
    CONSTRAINT [FK_TransactionAnnouncement_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transaction]([TransactionId]), 
)
GO

CREATE CLUSTERED INDEX [IX_TransactionAnnouncement] ON [dbo].[TransactionAnnouncement] ([TransactionId])
