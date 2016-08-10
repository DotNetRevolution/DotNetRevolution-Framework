CREATE TABLE [dbo].[TransactionInformation]
(
	[EventProviderTransactionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[Committed] DATETIME2 NOT NULL DEFAULT SYSDATETIME(), 
    [User] NVARCHAR(256) NOT NULL DEFAULT SUSER_SNAME(), 
    [Application] NVARCHAR(128) NOT NULL DEFAULT APP_NAME(), 
    
	CONSTRAINT [FK_TransactionInformation_EventProviderTransaction] FOREIGN KEY ([EventProviderTransactionId]) REFERENCES [dbo].[EventProviderTransaction]([EventProviderTransactionId])
)
