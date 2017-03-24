CREATE PROCEDURE [dbo].[GetAllAggregateRootTypes]
AS
	SELECT AggregateRootTypeId, FullName
	  FROM dbo.AggregateRootType
RETURN 0
