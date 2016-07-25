CREATE TABLE [dbo].[TransactionCommandType]
(
	[TransactionCommandTypeId] BINARY(16) NOT NULL PRIMARY KEY NONCLUSTERED, 
    [FullName] VARCHAR(512) NOT NULL
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_TransactionCommandType] ON [dbo].[TransactionCommandType] ([FullName])
