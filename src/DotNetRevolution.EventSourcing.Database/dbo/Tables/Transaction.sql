﻿CREATE TABLE [dbo].[Transaction]
(
	[TransactionId] BIGINT NOT NULL IDENTITY PRIMARY KEY, 
	[TransactionGuid] UNIQUEIDENTIFIER NOT NULL,
    [Processed] DATETIME2 NOT NULL DEFAULT SYSDATETIME(), 
    [User] NVARCHAR(256) NOT NULL DEFAULT SUSER_SNAME(), 
    [Application] NVARCHAR(128) NOT NULL DEFAULT APP_NAME(), 

    CONSTRAINT [AK_Transaction_TransactionGuid] UNIQUE ([TransactionGuid])
)

GO
