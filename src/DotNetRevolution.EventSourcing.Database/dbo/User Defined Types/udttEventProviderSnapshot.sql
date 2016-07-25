CREATE TYPE [dbo].[udttEventProviderSnapshot] AS TABLE
(
	  [TypeId]					BINARY(16)			NOT NULL
	, [TypeFullName]			VARCHAR (512)		NOT NULL
	, [Data]					VARBINARY (MAX)		NOT NULL
)
