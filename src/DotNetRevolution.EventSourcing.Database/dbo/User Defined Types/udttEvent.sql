CREATE TYPE [dbo].[udttEvent] AS TABLE 
(
      [EventProviderGuid]		UNIQUEIDENTIFIER NOT NULL
	, [EventId]					UNIQUEIDENTIFIER NOT NULL
    , [Sequence]				INT              NOT NULL
	, [TypeId]					BINARY(16)		 NOT NULL
	, [TypeFullName]			VARCHAR (512)    NOT NULL
    , [Data]					VARBINARY (MAX)  NOT NULL
);
