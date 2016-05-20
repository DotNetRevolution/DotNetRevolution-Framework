CREATE TYPE [dbo].[udttEventProviderSnapshot] AS TABLE
(
	  [EventProviderTempId]		INT				NOT NULL
	, [TypeFullName]			VARCHAR (512)	NOT NULL
	, [Data]					VARBINARY (MAX)	NOT NULL
)
