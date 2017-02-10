CREATE TABLE [dbo].[Projection]
(	
	[ProjectionId] UNIQUEIDENTIFIER NOT NULL,
	[ProjectionTypeId] BINARY(16) NOT NULL,	
	[EventProviderId] UNIQUEIDENTIFIER NOT NULL,
	[EventProviderVersion] INT NOT NULL,

    CONSTRAINT [FK_Projection_EventProvider] FOREIGN KEY ([EventProviderId]) REFERENCES [dbo].[EventProvider]([EventProviderId]),
    CONSTRAINT [FK_Projection_ProjectionType] FOREIGN KEY ([ProjectionTypeId]) REFERENCES [dbo].[ProjectionType]([ProjectionTypeId]),     
    CONSTRAINT [PK_Projection] PRIMARY KEY ([ProjectionTypeId],[ProjectionId],[EventProviderId]) 
)

GO
