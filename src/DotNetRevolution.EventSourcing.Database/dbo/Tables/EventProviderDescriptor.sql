CREATE TABLE [dbo].[EventProviderDescriptor]
(
    [EventProviderTransactionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Descriptor] VARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_EventProviderDescription_EventProviderTransaction] FOREIGN KEY ([EventProviderTransactionId]) REFERENCES [dbo].[EventProviderTransaction]([EventProviderTransactionId]), 
)

GO