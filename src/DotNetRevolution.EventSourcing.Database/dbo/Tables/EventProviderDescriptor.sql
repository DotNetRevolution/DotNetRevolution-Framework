CREATE TABLE [dbo].[EventProviderDescriptor]
(
    [TransactionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Descriptor] VARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_EventProviderDescription_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transaction]([TransactionId]), 
)

GO