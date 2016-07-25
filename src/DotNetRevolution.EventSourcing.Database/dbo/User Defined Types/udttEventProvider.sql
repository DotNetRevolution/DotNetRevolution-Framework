CREATE TYPE [dbo].[udttEventProvider] AS TABLE 
(
	  [Id]					UNIQUEIDENTIFIER NOT NULL
    , [Descriptor]			VARCHAR (MAX)    NOT NULL
	, [TypeId]				BINARY(16)		 NOT NULL
	, [TypeFullName]		VARCHAR (512)    NOT NULL
    , [Version]				INT              NOT NULL	
);
