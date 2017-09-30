IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Pagination]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[usp_Pagination]
GO

CREATE PROCEDURE [dbo].[usp_Pagination]
@tblName VARCHAR(4000),         -- base table + join table
@sortFields VARCHAR(200),       -- order by field with order mode(asc/desc)
@fields VARCHAR(1000) = '*',	-- 
@where VARCHAR(1000) = '',      -- condition, note no 'where'
@pageSize INT = 20,             -- 
@pageIndex INT = 1,             -- 
@totalCount INT OUTPUT          --
AS
DECLARE @strSql nVARCHAR(4000)
DECLARE @beginIndex  INT    
DECLARE @endIndex  INT

SET @beginIndex = @pageSize * (@pageIndex - 1);

-- get total count
IF @where !=''
    SET @strSql = N'SELECT @totalCount = ISNULL(count(1),0) FROM ' + @tblName + ' WHERE '+@where + ';';
ELSE
    SET @strSql = N'SELECT @totalCount = ISNULL(count(1),0) FROM ' + @tblName + ';';

SET @strSql = @strSql + 'SELECT ' + @fields + ' FROM ' + @tblName + '';
IF @where !=''
	SET @strSql = @strSql + ' WHERE ' + @where;

SET @strSql = @strSql + ' ORDER BY ' + @sortFields;

-- get records by page if pagesize > 0, else get all records
IF @pageSize > 0
    SET @strSql = @strSql + ' OFFSET ' + CAST (@beginIndex AS VARCHAR ) + ' ROWS FETCH NEXT ' + CAST (@pageSize AS VARCHAR ) + ' ROWS ONLY';

SET @strSql = @strSql + ';';

EXECUTE sp_executesql @strSql,N'@totalCount INT OUTPUT', @totalCount output;
