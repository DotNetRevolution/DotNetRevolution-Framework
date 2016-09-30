CREATE TABLE [dbo].[TransactionInformation]
(
	[EventProviderTransactionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[Committed] AS SYSUTCDATETIME() PERSISTED, 
    [User] NVARCHAR(256) NOT NULL DEFAULT SUSER_SNAME(), 
    [Application] AS APP_NAME() PERSISTED, 
    
	CONSTRAINT [FK_TransactionInformation_EventProviderTransaction] FOREIGN KEY ([EventProviderTransactionId]) REFERENCES [dbo].[EventProviderTransaction]([EventProviderTransactionId])
)
