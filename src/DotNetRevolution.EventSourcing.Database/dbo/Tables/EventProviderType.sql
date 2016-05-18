CREATE TABLE [dbo].[EventProviderType]
(
	[EventProviderTypeId] INT NOT NULL PRIMARY KEY, 
    [FullName] VARCHAR(512) NOT NULL, 
    CONSTRAINT [AK_EventProviderType_FullName] UNIQUE ([FullName]) 
)
