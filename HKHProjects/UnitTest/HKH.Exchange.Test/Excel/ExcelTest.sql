use master
go
create database ExcelTest
go
use ExcelTest
go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Grade]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Grade](
	[ClassId] [varchar](50) NOT NULL,
	[StudentId] [varchar](50) NOT NULL,
	[Net] [int] NULL,
	[Java] [int] NULL,
	[Html] [int] NULL,
 CONSTRAINT [PK_Grade] PRIMARY KEY CLUSTERED 
(
	[ClassId] ASC,
	[StudentId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
