CREATE TABLE [dbo].[DomainLibrary]
(
	[DomainLibraryId] INT NOT NULL PRIMARY KEY NONCLUSTERED IDENTITY,
	[DomainId] INT NOT NULL,
	[Name] VARCHAR(256) NOT NULL,
	[Description] VARCHAR(512) NOT NULL,
	[VersionMajor] INT NOT NULL,
	[VersionMinor] INT NOT NULL,
	[VersionBuild] INT NOT NULL,
	[VersionRevision] INT NOT NULL,
	[Library] VARBINARY(MAX) NOT NULL
)
