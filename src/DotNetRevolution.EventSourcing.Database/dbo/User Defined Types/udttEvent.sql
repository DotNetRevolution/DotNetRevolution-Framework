CREATE TYPE [dbo].[udttEvent] AS TABLE (
    [EventProviderGuid]	UNIQUEIDENTIFIER NOT NULL,
	[EventGuid]			UNIQUEIDENTIFIER NOT NULL,
    [Sequence]			INT              NOT NULL,
    [Type]				VARCHAR (512)    NOT NULL,
    [Data]				VARBINARY (MAX)  NOT NULL);

