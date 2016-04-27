CREATE TABLE [dbo].[TransactionCommand]
(
	[TransactionId] BIGINT NOT NULL PRIMARY KEY,
    [TransactionCommandTypeId] INT NOT NULL, 
    [TransactionCommandGuid] UNIQUEIDENTIFIER NOT NULL, 
    [Data] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_TransactionCommand_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transaction]([TransactionId]), 
    CONSTRAINT [FK_TransactionCommand_TransactionCommandType] FOREIGN KEY ([TransactionCommandTypeId]) REFERENCES [dbo].[TransactionCommandType]([TransactionCommandTypeId]), 
    CONSTRAINT [AK_TransactionCommand_CommandGuid] UNIQUE ([TransactionCommandGuid])
)
