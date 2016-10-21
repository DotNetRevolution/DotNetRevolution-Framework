CREATE TABLE [dbo].[TransactionInformation]
(
	[EventProviderTransactionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[Committed] AS SYSUTCDATETIME(), 
    [User] NVARCHAR(256) NOT NULL DEFAULT SUSER_SNAME(), 
    [Application] AS APP_NAME(), 
    
	CONSTRAINT [FK_TransactionInformation_EventProviderTransaction] FOREIGN KEY ([EventProviderTransactionId]) REFERENCES [dbo].[EventProviderTransaction]([EventProviderTransactionId])
)
