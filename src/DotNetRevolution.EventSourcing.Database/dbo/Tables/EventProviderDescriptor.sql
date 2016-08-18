CREATE TABLE [dbo].[EventProviderDescriptor]
(
    [EventProviderGuid] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Descriptor] VARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_EventProviderDescription_EventProvider] FOREIGN KEY ([EventProviderGuid]) REFERENCES [dbo].[EventProvider]([EventProviderGuid]), 
)

GO