CREATE TABLE [dbo].[EventProviderDescriptor]
(
    [EventProviderId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Descriptor] VARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_EventProviderDescription_EventProvider] FOREIGN KEY ([EventProviderId]) REFERENCES [dbo].[EventProvider]([EventProviderId]), 
)

GO