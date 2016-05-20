CREATE TYPE [dbo].[udttEvent] AS TABLE 
(
      [EventProviderTempId]		INT				 NOT NULL
	, [EventGuid]				UNIQUEIDENTIFIER NOT NULL
    , [Sequence]				INT              NOT NULL
	, [TypeFullName]			VARCHAR (512)    NOT NULL
    , [Data]					VARBINARY (MAX)  NOT NULL
);
