CREATE TABLE [dbo].[EventProviderDescriptor]
(
    [TransactionEventProviderId] BIGINT NOT NULL PRIMARY KEY,
    [Descriptor] VARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_EventProviderDescription_TransactionEventProvider] FOREIGN KEY ([TransactionEventProviderId]) REFERENCES [dbo].[TransactionEventProvider]([TransactionEventProviderId]), 
)

GO

CREATE NONCLUSTERED INDEX [IX_EventProviderDescriptor_TransactionId_EventProviderId] ON [dbo].[EventProviderDescriptor] ([TransactionEventProviderId]) INCLUDE ([Descriptor])