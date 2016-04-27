CREATE TABLE [dbo].[EventProviderDescriptor]
(
	[EventProviderDescriptorId] INT NOT NULL PRIMARY KEY IDENTITY,
    [EventProviderId] INT NOT NULL, 
    [TransactionId] BIGINT NOT NULL,
    [Descriptor] VARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_EventProviderDescription_EventProvider] FOREIGN KEY ([EventProviderId]) REFERENCES [dbo].[EventProvider]([EventProviderId]), 
    CONSTRAINT [FK_EventProviderDescription_Transaction] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[Transaction]([TransactionId]),     
    CONSTRAINT [AK_EventProviderDescription_EventProviderId_TransactionId] UNIQUE ([EventProviderId],[TransactionId]), 
)
