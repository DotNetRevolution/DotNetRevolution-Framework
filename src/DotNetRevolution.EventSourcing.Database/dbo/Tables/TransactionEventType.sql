CREATE TABLE [dbo].[TransactionEventType]
(
	[TransactionEventTypeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FullName] VARCHAR(512) NOT NULL, 
    CONSTRAINT [AK_TransactionEventType_FullName] UNIQUE ([FullName])
)
