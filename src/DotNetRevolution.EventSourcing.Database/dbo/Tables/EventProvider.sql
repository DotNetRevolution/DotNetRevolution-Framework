CREATE TABLE [dbo].[EventProvider]
(	
	[EventProviderId] INT NOT NULL PRIMARY KEY,
    [EventProviderTypeId] INT NOT NULL, 	
	[EventProviderGuid] UNIQUEIDENTIFIER NOT NULL,
	[CurrentEventProviderDescriptorId] BIGINT,
	[LatestSnapshotId] BIGINT,
	[CurrentVersion] INT NOT NULL DEFAULT (0),
    CONSTRAINT [FK_EventProvider_EventProviderType] FOREIGN KEY ([EventProviderTypeId]) REFERENCES [dbo].[EventProviderType]([EventProviderTypeId]), 
	CONSTRAINT [FK_EventProvider_CurrentEventProviderDescriptorId] FOREIGN KEY ([CurrentEventProviderDescriptorId]) REFERENCES [dbo].[EventProviderDescriptor]([TransactionEventProviderId]), 
	CONSTRAINT [FK_EventProvider_EventProviderSnapshotId] FOREIGN KEY ([LatestSnapshotId]) REFERENCES [dbo].[EventProviderSnapshot]([TransactionEventProviderId]), 
    CONSTRAINT [AK_EventProvider_EventProviderGuid_EventProviderTypeId] UNIQUE ([EventProviderGuid], [EventProviderTypeId]),     	
)

GO
