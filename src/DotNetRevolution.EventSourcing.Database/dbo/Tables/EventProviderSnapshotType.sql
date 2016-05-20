CREATE TABLE [dbo].[EventProviderSnapshotType]
(
	[EventProviderSnapshotTypeId] INT NOT NULL PRIMARY KEY, 
    [FullName] VARCHAR(512) NOT NULL, 
    CONSTRAINT [AK_EventProviderSnapshotType_FullName] UNIQUE ([FullName]) 
)
