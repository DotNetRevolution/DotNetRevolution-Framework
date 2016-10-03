﻿CREATE TABLE [dbo].[EventProviderTransaction]
(
	[EventProviderTransactionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED, 	
	[EventProviderGuid] UNIQUEIDENTIFIER NOT NULL,
	[EventProviderVersion] INT NOT NULL,
 
    CONSTRAINT [AK_EventProviderTransaction_EventProviderId_EventProviderVersion] UNIQUE ([EventProviderGuid],[EventProviderVersion])
)

GO

CREATE CLUSTERED INDEX [IX_EventProviderTransaction] ON [dbo].[EventProviderTransaction] ([EventProviderGuid])

GO