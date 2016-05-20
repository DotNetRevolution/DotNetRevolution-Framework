CREATE TYPE [dbo].[udttEventProvider] AS TABLE 
(
	  [TempId]				INT				 NOT NULL
    , [EventProviderGuid]	UNIQUEIDENTIFIER NOT NULL
    , [Descriptor]			VARCHAR (MAX)    NOT NULL
	, [TypeFullName]		VARCHAR (512)    NOT NULL
    , [Version]				INT              NOT NULL	
);
