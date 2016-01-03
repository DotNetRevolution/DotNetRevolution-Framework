CREATE TABLE [log].[Entry]
(
	[EntryId] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[Timestamp] DATETIME NOT NULL,
	[Level] VARCHAR(20) NOT NULL,
	[SourceContext] VARCHAR(256) NOT NULL,
	[Message] VARCHAR(MAX) NOT NULL,
	[Exception] VARCHAR(MAX) NULL,
    [MachineName] AS HOST_NAME() PERSISTED,    
    [Application] AS APP_NAME() PERSISTED, 
    [ConnectionUser] AS SUSER_SNAME() PERSISTED, 
    [ApplicationUser] VARCHAR(100) NOT NULL, 
	[SessionId] VARCHAR(256) NOT NULL,
	[Properties] XML NULL
)
