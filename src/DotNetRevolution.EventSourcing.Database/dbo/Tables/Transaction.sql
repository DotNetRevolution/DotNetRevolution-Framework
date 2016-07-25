CREATE TABLE [dbo].[Transaction]
(
	[TransactionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED, 	
	[EventProviderGuid] UNIQUEIDENTIFIER NOT NULL,
	[EventProviderVersion] INT NOT NULL,
    [Committed] DATETIME2 NOT NULL DEFAULT SYSDATETIME(), 
    [User] NVARCHAR(256) NOT NULL DEFAULT SUSER_SNAME(), 
    [Application] NVARCHAR(128) NOT NULL DEFAULT APP_NAME(),

    CONSTRAINT [AK_Transaction_EventProviderId_EventProviderVersion] UNIQUE ([EventProviderGuid],[EventProviderVersion])
)

GO

CREATE CLUSTERED INDEX [IX_Transaction] ON [dbo].[Transaction] ([EventProviderGuid])

GO