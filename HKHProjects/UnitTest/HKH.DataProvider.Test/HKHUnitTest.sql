USE [master]
GO
CREATE DATABASE [HKHUnitTest]
GO
USE [HKHUnitTest]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MasterTable](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NULL,
	[CreatedTime] [datetime] NULL,
	[Creator] [int] NULL,
 CONSTRAINT [PK_Master] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SubTable](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MasterID] [uniqueidentifier] NULL,
	[Memo] [varchar](50) NULL,
 CONSTRAINT [PK_SubTable] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO

IF OBJECT_ID(N'[dbo].[usp_MasterTable]') IS NOT NULL
	DROP PROCEDURE [dbo].[usp_MasterTable]
GO

------------------------------------------------------------------------------------------------------------------------
-- Name: [dbo].[usp_MasterTable]
-- Description: Insert/Delete/Update MasterTable in batches
-- @ItemsXml: must be Nvarchar/Ntext, else can't process chinese
-- @Operation: 
--			0 - Insert
--			1 - Delete
--			2 - Update
-- Created by: JackyLi	2013-03-29
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[usp_MasterTable]
	@ItemsXml NText,
	@Operation SmallInt
AS
BEGIN
	DECLARE @hXml INT;
	DECLARE @result INT;
	DECLARE @rtnValue INT;

	EXEC @result = sp_xml_preparedocument @hXml output,@ItemsXml;

	IF @result = 0
	BEGIN
		--Insert
		IF @Operation=0
		BEGIN
			INSERT INTO [dbo].[MasterTable](
					[ID],
					[Name],
					[CreatedTime],
					[Creator]
				)
				SELECT 
					[NewID],
					[NewName],
					[NewCreatedTime],
					[NewCreator]
				FROM 
				OPENXML(@hXml,'/Items/MasterTable',3)
				WITH
				(
					[NewID] uniqueidentifier 'ID',
					[NewName] varchar(50) 'Name',
					[NewCreatedTime] datetime 'CreatedTime',
					[NewCreator] int 'Creator'
				)
				AS TempTable
		END

		--Delete
		IF @Operation=1
		BEGIN
			DELETE [dbo].[MasterTable]
				FROM 
				OPENXML(@hXml,'/Items/MasterTable',3)
				WITH
				(
					[OriginalID] uniqueidentifier 'originalID'
				)
				AS TempTable
				WHERE 
					[ID] = [OriginalID]
		END

		--Update
		IF @Operation=2
		BEGIN
			UPDATE [dbo].[MasterTable] SET 
					[ID] = [NewID],
					[Name] = [NewName],
					[CreatedTime] = [NewCreatedTime],
					[Creator] = [NewCreator]
				FROM 
				OPENXML(@hXml,'/Items/MasterTable',3)
				WITH
				(
					[NewID] uniqueidentifier 'ID',
					[NewName] varchar(50) 'Name',
					[NewCreatedTime] datetime 'CreatedTime',
					[NewCreator] int 'Creator',
					[OriginalID] uniqueidentifier 'originalID'
				)
				AS TempTable
				WHERE 
					[ID] = [OriginalID]
		END

		SELECT @rtnValue = COUNT(1) FROM OPENXML(@hXml,'/Items/MasterTable',2);
		EXEC sp_xml_removedocument @hXml;
	END

	IF @@ERROR > 0
		RETURN -1;

	RETURN @rtnValue;
END

GO


IF OBJECT_ID(N'[dbo].[usp_SubTable]') IS NOT NULL
	DROP PROCEDURE [dbo].[usp_SubTable]
GO

------------------------------------------------------------------------------------------------------------------------
-- Name: [dbo].[usp_SubTable]
-- Description: Insert/Delete/Update SubTable in batches
-- @ItemsXml: must be Nvarchar/Ntext, else can't process chinese
-- @Operation: 
--			0 - Insert
--			1 - Delete
--			2 - Update
-- Created by: JackyLi	2013-03-29
------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[usp_SubTable]
	@ItemsXml NText,
	@Operation SmallInt
AS
BEGIN
	DECLARE @hXml INT;
	DECLARE @result INT;
	DECLARE @rtnValue INT;

	EXEC @result = sp_xml_preparedocument @hXml output,@ItemsXml;

	IF @result = 0
	BEGIN
		--Insert
		IF @Operation=0
		BEGIN
			INSERT INTO [dbo].[SubTable](
					[MasterID],
					[Memo]
				)
				SELECT 
					[NewMasterID],
					[NewMemo]
				FROM 
				OPENXML(@hXml,'/Items/SubTable',3)
				WITH
				(
					[NewID] bigint 'ID',
					[NewMasterID] uniqueidentifier 'MasterID',
					[NewMemo] varchar(50) 'Memo'
				)
				AS TempTable
		END

		--Delete
		IF @Operation=1
		BEGIN
			DELETE [dbo].[SubTable]
				FROM 
				OPENXML(@hXml,'/Items/SubTable',3)
				WITH
				(
					[OriginalID] bigint 'originalID'
				)
				AS TempTable
				WHERE 
					[ID] = [OriginalID]
		END

		--Update
		IF @Operation=2
		BEGIN
			UPDATE [dbo].[SubTable] SET 
					[MasterID] = [NewMasterID],
					[Memo] = [NewMemo]
				FROM 
				OPENXML(@hXml,'/Items/SubTable',3)
				WITH
				(
					[NewID] bigint 'ID',
					[NewMasterID] uniqueidentifier 'MasterID',
					[NewMemo] varchar(50) 'Memo',
					[OriginalID] bigint 'originalID'
				)
				AS TempTable
				WHERE 
					[ID] = [OriginalID]
		END

		SELECT @rtnValue = COUNT(1) FROM OPENXML(@hXml,'/Items/SubTable',2);
		EXEC sp_xml_removedocument @hXml;
	END

	IF @@ERROR > 0
		RETURN -1;

	IF @Operation = 0
		RETURN @@IDENTITY;
	ELSE
		RETURN @rtnValue;
END

GO


