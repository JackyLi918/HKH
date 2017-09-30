IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Pagination]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[usp_Pagination]
GO

CREATE PROCEDURE [dbo].[usp_Pagination]
@tblName VARCHAR(255),          -- base table + join table
@sortFields VARCHAR(255),        -- order by field with order mode(asc/desc)
@fields VARCHAR(1000) = '*',	-- 
@where VARCHAR(1500) = '',      -- condition, note no 'where'
@pageSize INT = 20,             -- 
@pageIndex INT = 1,             -- 
@totalCount INT OUTPUT          --
AS
DECLARE @strSql nVARCHAR(4000)
DECLARE @beginIndex  INT    
DECLARE @endIndex  INT

SET @beginIndex = @pageSize * (@pageIndex - 1) + 1;
SET @endIndex = @pageSize * @pageIndex;

-- get total count
IF @where !=''
    SET @strSql = N'SELECT @totalCount = ISNULL(count(1),0) FROM ' + @tblName + ' WHERE '+@where + ';';
ELSE
    SET @strSql = N'SELECT @totalCount = ISNULL(count(1),0) FROM ' + @tblName + ';';

-- get records by page if pagesize > 0, else get all records
IF @pageSize > 0
BEGIN
    SET @strSql = @strSql + 'SELECT * FROM ( SELECT ' + @fields + ',ROW_NUMBER() OVER (ORDER BY ' + @sortFields + ') AS ROWINDEX FROM ' + @tblName + '';

    IF @where !=''
        SET @strSql = @strSql + ' WHERE ' + @where;

    SET @strSql = @strSql + ') AS tempTable WHERE ROWINDEX BETWEEN ' + CAST (@beginIndex AS VARCHAR ) + ' AND ' + CAST (@endIndex AS VARCHAR ) + ';';
END
ELSE
BEGIN
    SET @strSql = @strSql + 'SELECT ' + @fields + ' FROM ' + @tblName + '';

    IF @where !=''
        SET @strSql = @strSql + ' WHERE ' + @where;

    SET @strSql = @strSql + ';';
END

EXECUTE sp_executesql @strSql,N'@totalCount INT OUTPUT', @totalCount output;
