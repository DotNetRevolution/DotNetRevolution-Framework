CREATE TABLE [dbo].[ProjectionLog]
(
	[ProjectionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[EventProviderTransactionId] UNIQUEIDENTIFIER NOT NULL,
	[Projected]	DATETIME2 NOT NULL, 
    CONSTRAINT [FK_ProjectionLog_EventProviderTransaction] FOREIGN KEY ([EventProviderTransactionId]) REFERENCES [dbo].[EventProviderTransaction]([EventProviderTransactionId])
)
