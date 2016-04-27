CREATE TYPE [dbo].[udttEventProvider] AS TABLE (
    [EventProviderGuid]		UNIQUEIDENTIFIER NOT NULL,
    [Descriptor]			VARCHAR (MAX)    NOT NULL,
    [Type]					VARCHAR (512)    NOT NULL,
    [Version]				INT              NOT NULL);
