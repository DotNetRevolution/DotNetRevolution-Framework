CREATE TYPE [dbo].[udttEvent] AS TABLE 
(
	  [EventId]					UNIQUEIDENTIFIER NOT NULL
    , [Sequence]				INT              NOT NULL
	, [TypeId]					BINARY(16)		 NOT NULL
	, [TypeFullName]			VARCHAR (512)    NOT NULL
    , [Data]					VARBINARY (MAX)  NOT NULL
);
