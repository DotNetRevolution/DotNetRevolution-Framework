﻿CREATE TABLE [dbo].[Transaction]
(
	[TransactionId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[Metadata] VARBINARY(MAX) NOT NULL,
	[EventProviderDescriptor] VARCHAR(MAX) NOT NULL
)
