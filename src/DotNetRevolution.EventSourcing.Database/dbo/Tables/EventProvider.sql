CREATE TABLE [dbo].[EventProvider]
(	
	[EventProviderId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY NONCLUSTERED,
    [AggregateRootTypeId] BINARY(16) NOT NULL, 	
	[AggregateRootId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_EventProvider_AggregateRootType] FOREIGN KEY ([AggregateRootTypeId]) REFERENCES [dbo].[AggregateRootType]([AggregateRootTypeId])
)

GO

CREATE UNIQUE CLUSTERED INDEX [IX_EventProvider] ON [dbo].[EventProvider] ([AggregateRootTypeId],[AggregateRootId])
