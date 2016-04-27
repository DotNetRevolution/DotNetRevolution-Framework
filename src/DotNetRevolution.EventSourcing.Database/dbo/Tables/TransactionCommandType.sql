CREATE TABLE [dbo].[TransactionCommandType]
(
	[TransactionCommandTypeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FullName] VARCHAR(512) NOT NULL, 
    CONSTRAINT [AK_TransactionCommandType_FullName] UNIQUE ([FullName])
)
