CREATE TABLE [dbo].[TransactionAnnouncement]
(
    [TransactionAnnouncementId] BIGINT NOT NULL PRIMARY KEY IDENTITY,
    [TransactionId] BIGINT NOT NULL, 
	[Announced] DATETIME NOT NULL, 
    CONSTRAINT [FK_TransactionAnnouncement_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transaction]([TransactionId]), 
)