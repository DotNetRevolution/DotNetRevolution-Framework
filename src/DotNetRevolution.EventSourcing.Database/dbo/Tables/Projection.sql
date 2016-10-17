CREATE TABLE [dbo].[Projection]
(
	[ProjectionId]	BINARY(16) NOT NULL PRIMARY KEY NONCLUSTERED,
	[FullName]		VARCHAR(512) NOT NULL	
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_Projection] ON [dbo].[Projection] ([FullName])