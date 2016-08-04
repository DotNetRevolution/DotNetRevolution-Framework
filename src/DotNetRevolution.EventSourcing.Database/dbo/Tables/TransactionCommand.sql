CREATE TABLE [dbo].[TransactionCommand]
(
	[EventProviderTransactionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED,
    [TransactionCommandTypeId] BINARY(16) NOT NULL, 
    [TransactionCommandId] UNIQUEIDENTIFIER NOT NULL, 
    [Data] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_TransactionCommand_EventProviderTransaction] FOREIGN KEY ([EventProviderTransactionId]) REFERENCES [dbo].[EventProviderTransaction]([EventProviderTransactionId]), 
    CONSTRAINT [FK_TransactionCommand_TransactionCommandType] FOREIGN KEY ([TransactionCommandTypeId]) REFERENCES [dbo].[TransactionCommandType]([TransactionCommandTypeId]), 
    CONSTRAINT [AK_TransactionCommand_CommandGuid] UNIQUE ([TransactionCommandId])
)

GO

CREATE CLUSTERED INDEX [IX_TransactionCommand] ON [dbo].[TransactionCommand] ([TransactionCommandTypeId])
