IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Pagination]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[usp_Pagination]
GO

CREATE PROCEDURE [dbo].[usp_Pagination]
@sql NVARCHAR(4000),
@orderBy NVARCHAR(100),
@pageSize INT = 20,
@pageIndex INT = 1,
@totalCount INT OUTPUT
AS
DECLARE @strSql NVARCHAR(4000)
DECLARE @beginIndex  INT    
DECLARE @endIndex  INT

SET @beginIndex = @pageSize * (@pageIndex - 1);

-- get total count
SET @strSql = N'SELECT @totalCount = ISNULL(count(1),0) FROM (' + @sql + ') AS tmp;';

-- get records by page if pagesize > 0, else get all records
IF @pageSize > 0
    SET @strSql = @strSql + @sql + ' ORDER BY ' + @orderBy + ' OFFSET ' + CAST (@beginIndex AS VARCHAR ) + ' ROWS FETCH NEXT ' + CAST (@pageSize AS VARCHAR ) + ' ROWS ONLY';

print @strSql 

EXECUTE sp_executesql @strSql,N'@totalCount INT OUTPUT', @totalCount output;
