CREATE TABLE [dbo].[TransactionCommand]
(
	[TransactionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED,
    [TransactionCommandTypeId] BINARY(16) NOT NULL, 
    [TransactionCommandId] UNIQUEIDENTIFIER NOT NULL, 
    [Data] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_TransactionCommand_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transaction]([TransactionId]), 
    CONSTRAINT [FK_TransactionCommand_TransactionCommandType] FOREIGN KEY ([TransactionCommandTypeId]) REFERENCES [dbo].[TransactionCommandType]([TransactionCommandTypeId]), 
    CONSTRAINT [AK_TransactionCommand_CommandGuid] UNIQUE ([TransactionCommandId])
)

GO

CREATE CLUSTERED INDEX [IX_TransactionCommand] ON [dbo].[TransactionCommand] ([TransactionCommandTypeId])
