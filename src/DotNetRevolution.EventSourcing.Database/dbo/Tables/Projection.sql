CREATE TABLE [dbo].[Projection]
(
	[ProjectionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED,
	[ProjectionTypeId] BINARY(16) NOT NULL
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_Projection] ON [dbo].[Projection] ([ProjectionTypeId])