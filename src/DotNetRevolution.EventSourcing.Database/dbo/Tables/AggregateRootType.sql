CREATE TABLE [dbo].[AggregateRootType]
(
	[AggregateRootTypeId] BINARY(16) NOT NULL PRIMARY KEY NONCLUSTERED, 
    [FullName] VARCHAR(512) NOT NULL,
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_AggregateRoot] ON [dbo].[AggregateRootType] ([FullName])
