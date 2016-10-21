CREATE TABLE [dbo].[ProjectionType]
(
	[ProjectionTypeId] BINARY(16) NOT NULL PRIMARY KEY NONCLUSTERED,
	[FullName] VARCHAR(512) NOT NULL	
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_ProjectionType] ON [dbo].[ProjectionType] ([FullName])