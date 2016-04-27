CREATE TABLE [dbo].[TransactionAnnouncementError]
(
	[TransactionAnnouncementErrorId] BIGINT NOT NULL PRIMARY KEY IDENTITY,	
	[TransactionAnnouncementId] BIGINT NOT NULL,
	[TransactionEventId] BIGINT NOT NULL, 
	[ErrorType] VARCHAR(MAX) NOT NULL,
	[ErrorMessage] VARCHAR(MAX) NOT NULL,
    CONSTRAINT [FK_TransactionAnnouncementError_TransactionAnnouncement] FOREIGN KEY ([TransactionAnnouncementId]) REFERENCES [dbo].[TransactionAnnouncement]([TransactionAnnouncementId]),
    CONSTRAINT [FK_TransactionAnnouncementError_TransactionEvent] FOREIGN KEY ([TransactionEventId]) REFERENCES [dbo].[TransactionEvent]([TransactionEventId])
)
