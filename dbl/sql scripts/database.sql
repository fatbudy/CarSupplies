USE [master]
GO

--…Ë÷√CLR∆Ù”√
SP_CONFIGURE 'clr enable',1
GO
RECONFIGURE
GO


declare @SmoRoot nvarchar(512)
declare @dbfile	nvarchar(512)
declare @dlfile	nvarchar(512)
--exec master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'SOFTWARE\Microsoft\MSSQLServer\Setup', N'SQLPath', @SmoRoot OUTPUT
exec master.dbo.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'SOFTWARE\Microsoft\MSSQLServer\MSSQLServer', N'DefaultData', @SmoRoot OUTPUT

select @dbfile=@smoroot+'\Carsupplies.mdf',@dlfile=@smoroot+'\Carsupplies_log.ldf'

declare @sql nvarchar(400)
select @sql='
CREATE DATABASE [Carsupplies] ON  PRIMARY 
( NAME = N''Carsupplies'', FILENAME =N'''+@dbfile+''' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N''Carsupplies_log'', FILENAME = N'''+@dlfile+''' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
'
exec sp_executesql @sql
if(@@error!=0)
	return
go

ALTER DATABASE [Carsupplies] SET COMPATIBILITY_LEVEL = 100
GO

USE [Carsupplies]
GO
/****** Object:  UserDefinedFunction [dbo].[getNewUid]    Script Date: 10/27/2015 16:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[getNewUid]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[getNewUid]()
returns nvarchar(4)
as
begin
	declare @maxid int
	select @maxid=max(cast(uid as int)) from users
	
	if(@maxid is null or @maxid<1000)
	begin
		set @maxid=1000
	end
	else
	begin
		set @maxid=@maxid+1
	end
	return cast(@maxid as nvarchar(4))
end' 
END
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[uid] [nvarchar](4) NULL,
	[uname] [nvarchar](50) NOT NULL,
	[code] [nvarchar](100) NOT NULL,
	[permission] [ntext] NULL,
	[levels] [tinyint] NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ActionKeys]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActionKeys]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActionKeys](
	[key] [nvarchar](50) NOT NULL,
	[value] [ntext] NOT NULL,
	[setsql] [nvarchar](512) NULL,
	[author] [nvarchar](50) NULL,
	[craetedatetime] [datetime] NULL,
	[summary] [nvarchar](200) NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ActionKeys] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ActionGroupList]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActionGroupList]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActionGroupList](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[akid] [int] NOT NULL,
	[assid] [int] NOT NULL,
 CONSTRAINT [PK_ActionGroupList] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ActionColumnSettingGroup]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActionColumnSettingGroup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActionColumnSettingGroup](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[assid] [int] NOT NULL,
	[acsid] [int] NOT NULL,
 CONSTRAINT [PK_ActionColumnSettingGroup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ActionColumnSetting]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActionColumnSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActionColumnSetting](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ColName] [nvarchar](50) NULL,
	[Visiable] [bit] NULL,
	[HeadText] [nvarchar](100) NULL,
	[Width] [int] NULL,
	[Localtion] [int] NULL,
	[LinkDataSqlString] [int] NULL,
	[LinkData] [bit] NULL,
	[LinkColumnName] [nvarchar](50) NULL,
 CONSTRAINT [PK_ActionColumnSetting] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[emplyeeEx]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[emplyeeEx]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[emplyeeEx](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[dutyid] [int] NOT NULL,
	[businessMark] [bit] NULL
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Emplyee]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Emplyee]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Emplyee](
	[eid] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[sex] [nvarchar](1) NULL,
	[age] [tinyint] NULL,
	[birthday] [datetime] NULL,
	[dept] [nvarchar](20) NULL,
	[dutyExId] [int] NULL,
	[mobile] [nchar](11) NULL,
	[phone] [nchar](13) NULL,
	[address] [nvarchar](255) NULL,
	[uid] [nvarchar](4) NULL,
	[QQ] [nvarchar](20) NULL,
	[email] [nvarchar](200) NULL,
	[weibo] [nvarchar](200) NULL,
 CONSTRAINT [PK_emplyee] PRIMARY KEY CLUSTERED 
(
	[eid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Customs]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Customs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cname] [nvarchar](200) NOT NULL,
	[caddress] [nvarchar](200) NULL,
	[cwebsite] [nvarchar](200) NULL,
	[cphone] [nvarchar](13) NULL,
	[cmobile] [nchar](11) NOT NULL,
	[cfax] [nvarchar](13) NULL,
	[cemail] [nvarchar](200) NULL,
	[cCarID] [nvarchar](50) NOT NULL,
	[cVip] [bit] NULL,
	[cVipCode] [nvarchar](20) NULL,
	[cVipdiscount] [float] NULL,
	[cdiscount] [float] NULL,
 CONSTRAINT [PK_Customs] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CustomMessageList]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomMessageList]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CustomMessageList](
	[id] [int] NOT NULL,
	[caption] [nvarchar](50) NULL,
	[text] [nvarchar](255) NOT NULL,
	[icon] [int] NULL,
	[button] [tinyint] NULL,
 CONSTRAINT [PK_CustomMessageList] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[contactEx]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[contactEx]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[contactEx](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[eid] [int] NOT NULL,
	[contactType] [nvarchar](20) NOT NULL,
	[value] [nvarchar](200) NULL,
 CONSTRAINT [PK_contactEx] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[BusinessSub]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BusinessSub]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BusinessSub](
	[id] [int] NOT NULL,
	[bid] [int] NOT NULL,
	[pid] [nvarchar](20) NOT NULL,
	[pname] [nvarchar](100) NULL,
	[sd] [nvarchar](20) NULL,
	[code] [nvarchar](50) NULL,
	[unit] [nvarchar](5) NOT NULL,
	[number] [float] NOT NULL,
	[price] [float] NOT NULL,
	[totalprice] [float] NULL,
	[note] [ntext] NULL,
 CONSTRAINT [PK_BusinessSub] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Business]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Business]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Business](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[bInOut] [bit] NOT NULL,
	[bType] [nvarchar](50) NOT NULL,
	[bDate] [datetime] NOT NULL,
	[bName] [nvarchar](50) NOT NULL,
	[bCustom] [nvarchar](50) NOT NULL,
	[bCID] [int] NOT NULL,
	[bStoreId] [int] NOT NULL,
	[bNote] [ntext] NULL,
	[bCUName] [nvarchar](50) NOT NULL,
	[bBalance] [bit] NOT NULL,
	[bsd] [nvarchar](20) NULL,
 CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[proGroup]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proGroup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[proGroup](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[groupname] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Products]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Products](
	[id] [nvarchar](20) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[category] [int] NULL,
	[groupid] [int] NULL,
	[pic] [image] NULL,
	[unit] [nvarchar](5) NULL,
	[price] [float] NULL,
	[mulitunit] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[proCategory]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proCategory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[proCategory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Stores]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Stores]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Stores](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[syear] [int] NOT NULL,
	[smonth] [tinyint] NOT NULL,
	[storeid] [int] NOT NULL,
	[pid] [nvarchar](20) NOT NULL,
	[sd] [nvarchar](20) NULL,
	[unit] [nvarchar](5) NULL,
	[lastbalance] [float] NULL,
	[lastout] [float] NULL,
	[lastin] [float] NULL,
	[currentin] [float] NULL,
	[currentout] [float] NULL,
	[number] [float] NULL,
 CONSTRAINT [PK_stores] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[storeEx]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[storeEx]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[storeEx](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sName] [nvarchar](50) NOT NULL,
	[sturct] [int] NULL
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Reserve]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reserve]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Reserve](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[rcid] [int] NOT NULL,
	[rbid] [int] NOT NULL,
	[rstd] [datetime] NOT NULL,
	[redd] [datetime] NOT NULL,
	[rCash] [bit] NOT NULL,
	[rtatal] [float] NULL,
	[rReserveCase] [float] NOT NULL,
	[rnote] [ntext] NULL,
 CONSTRAINT [PK_Reserve] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[proUnitsd]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proUnitsd]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[proUnitsd](
	[id] [int] NOT NULL,
	[pid] [nvarchar](20) NOT NULL,
	[sd] [nvarchar](20) NOT NULL,
	[total] [float] NOT NULL,
	[currentnumber] [float] NOT NULL,
	[completed] [bit] NULL,
 CONSTRAINT [PK_proUnitsd] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[proSubUnits]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proSubUnits]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[proSubUnits](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[pid] [nvarchar](20) NOT NULL,
	[unit] [nvarchar](5) NOT NULL,
	[number] [float] NULL,
 CONSTRAINT [PK_proSubUnits] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[getSelect]    Script Date: 10/27/2015 16:54:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[getSelect]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'create function [dbo].[getSelect](@tbname nvarchar(200))
returns nvarchar(4000)
as
begin

declare @tmp nvarchar(4000)
select @tmp=''SELECT ''
select @tmp=@tmp +''[''+c.name +''],'' from syscolumns as C,sysobjects as O where c.id=o.id and o.name=@tbname order by c.colid 
select @tmp=substring(@tmp,1,len(@tmp)-1)+'' from ''+@tbname
return @tmp
end ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[get_param]    Script Date: 10/27/2015 16:54:31 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[get_param]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[get_param](@s [nvarchar](4000))
RETURNS [int] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [SqlServerProject1].[UserDefinedFunctions].[GetParam]' 
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'SqlAssemblyFile' , N'SCHEMA',N'dbo', N'FUNCTION',N'get_param', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'SqlAssemblyFile', @value=N'Function1.cs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'get_param'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'SqlAssemblyFileLine' , N'SCHEMA',N'dbo', N'FUNCTION',N'get_param', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'SqlAssemblyFileLine', @value=N'14' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'get_param'
GO
/****** Object:  Table [dbo].[proGroupItems]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proGroupItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[proGroupItems](
	[groupid] [int] NOT NULL,
	[categoryid] [int] NOT NULL
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[groupProductItem]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[groupProductItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[groupProductItem](
	[groupid] [int] NOT NULL,
	[proid] [nvarchar](20) NOT NULL
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ActionStepSQLItem]    Script Date: 10/27/2015 16:54:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActionStepSQLItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActionStepSQLItem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ActionName] [nvarchar](50) NOT NULL,
	[StepSQL] [nvarchar](2000) NULL,
	[paramnumber] [tinyint] NULL,
 CONSTRAINT [PK_ActionStepSQL] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  View [dbo].[groupCategory]    Script Date: 10/27/2015 16:54:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[groupCategory]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[groupCategory]
AS
SELECT     dbo.proGroup.groupname, dbo.proCategory.name
FROM         dbo.proGroup INNER JOIN
                      dbo.proGroupItems ON dbo.proGroup.id = dbo.proGroupItems.groupid INNER JOIN
                      dbo.proCategory ON dbo.proGroupItems.categoryid = dbo.proCategory.id
'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'groupCategory', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "proGroup"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 96
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "proGroupItems"
            Begin Extent = 
               Top = 6
               Left = 236
               Bottom = 96
               Right = 396
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "proCategory"
            Begin Extent = 
               Top = 6
               Left = 434
               Bottom = 96
               Right = 594
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'groupCategory'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'groupCategory', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'groupCategory'
GO
/****** Object:  View [dbo].[vAItemGroup]    Script Date: 10/27/2015 16:54:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vAItemGroup]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vAItemGroup]
AS
SELECT     dbo.ActionColumnSetting.ID, dbo.ActionColumnSetting.ColName, dbo.ActionColumnSetting.Visiable, dbo.ActionColumnSetting.HeadText, dbo.ActionColumnSetting.Width, 
                      dbo.ActionStepSQLItem.ActionName
FROM         dbo.ActionColumnSetting INNER JOIN
                      dbo.ActionColumnSettingGroup ON dbo.ActionColumnSetting.ID = dbo.ActionColumnSettingGroup.acsid INNER JOIN
                      dbo.ActionStepSQLItem ON dbo.ActionColumnSettingGroup.assid = dbo.ActionStepSQLItem.ID
'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'vAItemGroup', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ActionColumnSetting"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 126
               Right = 198
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActionColumnSettingGroup"
            Begin Extent = 
               Top = 121
               Left = 257
               Bottom = 226
               Right = 417
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActionStepSQLItem"
            Begin Extent = 
               Top = 6
               Left = 434
               Bottom = 126
               Right = 594
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vAItemGroup'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'vAItemGroup', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vAItemGroup'
GO
/****** Object:  UserDefinedFunction [dbo].[get_ActionSetting]    Script Date: 10/27/2015 16:54:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[get_ActionSetting]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE function [dbo].[get_ActionSetting]( @akid int)
returns @tb table
(
	id int,
	colname nvarchar(100),
	visiable bit,
	headtext nvarchar(100),
	width int,
	localtion int,
	linkdata bit,
	linkdatasqlstring int,
	LinkColumnName nvarchar(50),
	actionname nvarchar(100)
)
as
begin

insert into @tb
select c.[ID]
      ,c.[ColName]
      ,c.[Visiable]
      ,c.[HeadText]
      ,c.[Width]
      ,c.[Localtion],c.[LinkData]
      ,isnull(c.[LinkDataSqlString],'''')[LinkDataSqlString]
      ,LinkColumnName
      ,I.ActionName  
	from ActionColumnSetting as c,ActionStepSQLItem as I,ActionColumnSettingGroup as g ,ActionGroupList as L
	where g.acsid =c.ID and g.assid =I.id and g.assid =L.assid and L.akid =@akid 

return 
end' 
END
GO
/****** Object:  View [dbo].[groupAction]    Script Date: 10/27/2015 16:54:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[groupAction]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[groupAction]
AS
SELECT     dbo.ActionStepSQLItem.ActionName, dbo.ActionStepSQLItem.StepSQL, dbo.ActionStepSQLItem.paramnumber, dbo.ActionKeys.[key], dbo.ActionStepSQLItem.ID
FROM         dbo.ActionGroupList INNER JOIN
                      dbo.ActionStepSQLItem ON dbo.ActionGroupList.assid = dbo.ActionStepSQLItem.ID INNER JOIN
                      dbo.ActionKeys ON dbo.ActionGroupList.akid = dbo.ActionKeys.id
'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'groupAction', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ActionGroupList"
            Begin Extent = 
               Top = 9
               Left = 0
               Bottom = 214
               Right = 287
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActionStepSQLItem"
            Begin Extent = 
               Top = 0
               Left = 378
               Bottom = 209
               Right = 622
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActionKeys"
            Begin Extent = 
               Top = 20
               Left = 696
               Bottom = 208
               Right = 858
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1635
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'groupAction'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'groupAction', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'groupAction'
GO
/****** Object:  StoredProcedure [dbo].[getTableColumnsSetting]    Script Date: 10/27/2015 16:54:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[getTableColumnsSetting]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[getTableColumnsSetting](@tableName nvarchar(200))
as
declare @id int 
declare @iid int

select @iid=ID from ActionStepSQLItem where ActionName =@tableName
select @id=isnull(MAX(id),0) from ActionColumnSetting

insert into ActionColumnSetting (ColName,Visiable )	
	select C.name ,''1'' from syscolumns as C,sysobjects as O 
	where C.id =O.id and O.name =@tableName

insert into ActionColumnSettingGroup (assid,acsid)
	select @iid,ID from ActionColumnSetting where ID >@id' 
END
GO
/****** Object:  Default [DF_ActionColumnSetting_Visiable]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ActionColumnSetting_Visiable]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActionColumnSetting]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ActionColumnSetting_Visiable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ActionColumnSetting] ADD  CONSTRAINT [DF_ActionColumnSetting_Visiable]  DEFAULT ((1)) FOR [Visiable]
END


End
GO
/****** Object:  Default [DF_ActionColumnSetting_Width]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ActionColumnSetting_Width]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActionColumnSetting]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ActionColumnSetting_Width]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ActionColumnSetting] ADD  CONSTRAINT [DF_ActionColumnSetting_Width]  DEFAULT ((100)) FOR [Width]
END


End
GO
/****** Object:  Default [DF_ActionColumnSetting_Localtion]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ActionColumnSetting_Localtion]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActionColumnSetting]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ActionColumnSetting_Localtion]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ActionColumnSetting] ADD  CONSTRAINT [DF_ActionColumnSetting_Localtion]  DEFAULT ((0)) FOR [Localtion]
END


End
GO
/****** Object:  Default [DF_ActionColumnSetting_LinkData]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ActionColumnSetting_LinkData]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActionColumnSetting]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ActionColumnSetting_LinkData]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ActionColumnSetting] ADD  CONSTRAINT [DF_ActionColumnSetting_LinkData]  DEFAULT ((0)) FOR [LinkData]
END


End
GO
/****** Object:  Default [DF_ActionColumnSetting_LinkColumnName]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ActionColumnSetting_LinkColumnName]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActionColumnSetting]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ActionColumnSetting_LinkColumnName]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ActionColumnSetting] ADD  CONSTRAINT [DF_ActionColumnSetting_LinkColumnName]  DEFAULT ('') FOR [LinkColumnName]
END


End
GO
/****** Object:  Default [DF_ActionKeys_craetedatetime]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ActionKeys_craetedatetime]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActionKeys]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ActionKeys_craetedatetime]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ActionKeys] ADD  CONSTRAINT [DF_ActionKeys_craetedatetime]  DEFAULT (getdate()) FOR [craetedatetime]
END


End
GO
/****** Object:  Default [DF_ActionKeys_summary]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ActionKeys_summary]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActionKeys]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ActionKeys_summary]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ActionKeys] ADD  CONSTRAINT [DF_ActionKeys_summary]  DEFAULT ('') FOR [summary]
END


End
GO
/****** Object:  Default [DF_ActionStepSQLItem_param]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_ActionStepSQLItem_param]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActionStepSQLItem]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ActionStepSQLItem_param]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ActionStepSQLItem] ADD  CONSTRAINT [DF_ActionStepSQLItem_param]  DEFAULT ((0)) FOR [paramnumber]
END


End
GO
/****** Object:  Default [DF_Business_bDate]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Business_bDate]') AND parent_object_id = OBJECT_ID(N'[dbo].[Business]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Business_bDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Business] ADD  CONSTRAINT [DF_Business_bDate]  DEFAULT (getdate()) FOR [bDate]
END


End
GO
/****** Object:  Default [DF_Business_bBalance]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Business_bBalance]') AND parent_object_id = OBJECT_ID(N'[dbo].[Business]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Business_bBalance]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Business] ADD  CONSTRAINT [DF_Business_bBalance]  DEFAULT ((0)) FOR [bBalance]
END


End
GO
/****** Object:  Default [DF_BusinessSub_number]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_BusinessSub_number]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessSub]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BusinessSub_number]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BusinessSub] ADD  CONSTRAINT [DF_BusinessSub_number]  DEFAULT ((0)) FOR [number]
END


End
GO
/****** Object:  Default [DF_BusinessSub_price]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_BusinessSub_price]') AND parent_object_id = OBJECT_ID(N'[dbo].[BusinessSub]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BusinessSub_price]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[BusinessSub] ADD  CONSTRAINT [DF_BusinessSub_price]  DEFAULT ((0.0)) FOR [price]
END


End
GO
/****** Object:  Default [DF_CustomMessageList_caption]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CustomMessageList_caption]') AND parent_object_id = OBJECT_ID(N'[dbo].[CustomMessageList]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CustomMessageList_caption]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CustomMessageList] ADD  CONSTRAINT [DF_CustomMessageList_caption]  DEFAULT ('') FOR [caption]
END


End
GO
/****** Object:  Default [DF_CustomMessageList_icon]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CustomMessageList_icon]') AND parent_object_id = OBJECT_ID(N'[dbo].[CustomMessageList]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CustomMessageList_icon]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CustomMessageList] ADD  CONSTRAINT [DF_CustomMessageList_icon]  DEFAULT ((64)) FOR [icon]
END


End
GO
/****** Object:  Default [DF_CustomMessageList_buttion]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_CustomMessageList_buttion]') AND parent_object_id = OBJECT_ID(N'[dbo].[CustomMessageList]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CustomMessageList_buttion]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[CustomMessageList] ADD  CONSTRAINT [DF_CustomMessageList_buttion]  DEFAULT ((0)) FOR [button]
END


End
GO
/****** Object:  Default [DF_Customs_caddress]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Customs_caddress]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customs]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Customs_caddress]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Customs] ADD  CONSTRAINT [DF_Customs_caddress]  DEFAULT (N'°Æ°Ø') FOR [caddress]
END


End
GO
/****** Object:  Default [DF_Customs_cwebsite]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Customs_cwebsite]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customs]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Customs_cwebsite]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Customs] ADD  CONSTRAINT [DF_Customs_cwebsite]  DEFAULT (N'°Æ°Ø') FOR [cwebsite]
END


End
GO
/****** Object:  Default [DF_Customs_cphone]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Customs_cphone]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customs]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Customs_cphone]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Customs] ADD  CONSTRAINT [DF_Customs_cphone]  DEFAULT (N'°Æ°Ø') FOR [cphone]
END


End
GO
/****** Object:  Default [DF_Customs_cmobile]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Customs_cmobile]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customs]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Customs_cmobile]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Customs] ADD  CONSTRAINT [DF_Customs_cmobile]  DEFAULT (N'°Æ°Ø') FOR [cmobile]
END


End
GO
/****** Object:  Default [DF_Customs_cfax]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Customs_cfax]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customs]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Customs_cfax]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Customs] ADD  CONSTRAINT [DF_Customs_cfax]  DEFAULT (N'°Æ°Ø') FOR [cfax]
END


End
GO
/****** Object:  Default [DF_Customs_cemail]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Customs_cemail]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customs]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Customs_cemail]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Customs] ADD  CONSTRAINT [DF_Customs_cemail]  DEFAULT (N'°Æ°Ø') FOR [cemail]
END


End
GO
/****** Object:  Default [DF_Customs_cVip]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Customs_cVip]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customs]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Customs_cVip]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Customs] ADD  CONSTRAINT [DF_Customs_cVip]  DEFAULT ((0)) FOR [cVip]
END


End
GO
/****** Object:  Default [DF_Customs_cVipdiscount]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Customs_cVipdiscount]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customs]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Customs_cVipdiscount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Customs] ADD  CONSTRAINT [DF_Customs_cVipdiscount]  DEFAULT ((0.0)) FOR [cVipdiscount]
END


End
GO
/****** Object:  Default [DF_Customs_cdiscount]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Customs_cdiscount]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customs]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Customs_cdiscount]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Customs] ADD  CONSTRAINT [DF_Customs_cdiscount]  DEFAULT ((1.0)) FOR [cdiscount]
END


End
GO
/****** Object:  Default [DF_emplyee_sex]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_emplyee_sex]') AND parent_object_id = OBJECT_ID(N'[dbo].[Emplyee]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_emplyee_sex]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Emplyee] ADD  CONSTRAINT [DF_emplyee_sex]  DEFAULT ('ƒ–') FOR [sex]
END


End
GO
/****** Object:  Default [DF_emplyee_uid]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_emplyee_uid]') AND parent_object_id = OBJECT_ID(N'[dbo].[Emplyee]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_emplyee_uid]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Emplyee] ADD  CONSTRAINT [DF_emplyee_uid]  DEFAULT ('') FOR [uid]
END


End
GO
/****** Object:  Default [DF_emplyeeEx_businessMark]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_emplyeeEx_businessMark]') AND parent_object_id = OBJECT_ID(N'[dbo].[emplyeeEx]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_emplyeeEx_businessMark]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[emplyeeEx] ADD  CONSTRAINT [DF_emplyeeEx_businessMark]  DEFAULT ((0)) FOR [businessMark]
END


End
GO
/****** Object:  Default [DF__Products__catego__1273C1CD]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__Products__catego__1273C1CD]') AND parent_object_id = OBJECT_ID(N'[dbo].[Products]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Products__catego__1273C1CD]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [category]
END


End
GO
/****** Object:  Default [DF__Products__groupi__1367E606]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__Products__groupi__1367E606]') AND parent_object_id = OBJECT_ID(N'[dbo].[Products]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Products__groupi__1367E606]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [groupid]
END


End
GO
/****** Object:  Default [DF__Products__price__145C0A3F]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__Products__price__145C0A3F]') AND parent_object_id = OBJECT_ID(N'[dbo].[Products]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Products__price__145C0A3F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0.0)) FOR [price]
END


End
GO
/****** Object:  Default [DF_Products_mulituint]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Products_mulituint]') AND parent_object_id = OBJECT_ID(N'[dbo].[Products]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Products_mulituint]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_mulituint]  DEFAULT ((0)) FOR [mulitunit]
END


End
GO
/****** Object:  Default [DF_proUnitsd_completed]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_proUnitsd_completed]') AND parent_object_id = OBJECT_ID(N'[dbo].[proUnitsd]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_proUnitsd_completed]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[proUnitsd] ADD  CONSTRAINT [DF_proUnitsd_completed]  DEFAULT ((0)) FOR [completed]
END


End
GO
/****** Object:  Default [DF_Reserve_rCash]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Reserve_rCash]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reserve]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Reserve_rCash]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Reserve] ADD  CONSTRAINT [DF_Reserve_rCash]  DEFAULT ((0)) FOR [rCash]
END


End
GO
/****** Object:  Default [DF_Reserve_rtatal]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Reserve_rtatal]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reserve]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Reserve_rtatal]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Reserve] ADD  CONSTRAINT [DF_Reserve_rtatal]  DEFAULT ((0.0)) FOR [rtatal]
END


End
GO
/****** Object:  Default [DF_Reserve_rReserveCase]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Reserve_rReserveCase]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reserve]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Reserve_rReserveCase]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Reserve] ADD  CONSTRAINT [DF_Reserve_rReserveCase]  DEFAULT ((0.0)) FOR [rReserveCase]
END


End
GO
/****** Object:  Default [DF_storeEx_sturct]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_storeEx_sturct]') AND parent_object_id = OBJECT_ID(N'[dbo].[storeEx]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_storeEx_sturct]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[storeEx] ADD  CONSTRAINT [DF_storeEx_sturct]  DEFAULT ((0)) FOR [sturct]
END


End
GO
/****** Object:  Default [DF_stores_lastbalance]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_stores_lastbalance]') AND parent_object_id = OBJECT_ID(N'[dbo].[Stores]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_stores_lastbalance]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Stores] ADD  CONSTRAINT [DF_stores_lastbalance]  DEFAULT ((0.0)) FOR [lastbalance]
END


End
GO
/****** Object:  Default [DF_stores_lastout]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_stores_lastout]') AND parent_object_id = OBJECT_ID(N'[dbo].[Stores]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_stores_lastout]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Stores] ADD  CONSTRAINT [DF_stores_lastout]  DEFAULT ((0.0)) FOR [lastout]
END


End
GO
/****** Object:  Default [DF_stores_lastin]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_stores_lastin]') AND parent_object_id = OBJECT_ID(N'[dbo].[Stores]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_stores_lastin]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Stores] ADD  CONSTRAINT [DF_stores_lastin]  DEFAULT ((0.0)) FOR [lastin]
END


End
GO
/****** Object:  Default [DF_stores_currentin]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_stores_currentin]') AND parent_object_id = OBJECT_ID(N'[dbo].[Stores]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_stores_currentin]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Stores] ADD  CONSTRAINT [DF_stores_currentin]  DEFAULT ((0.0)) FOR [currentin]
END


End
GO
/****** Object:  Default [DF_stores_currentout]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_stores_currentout]') AND parent_object_id = OBJECT_ID(N'[dbo].[Stores]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_stores_currentout]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Stores] ADD  CONSTRAINT [DF_stores_currentout]  DEFAULT ((0.0)) FOR [currentout]
END


End
GO
/****** Object:  Default [DF_stores_number]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_stores_number]') AND parent_object_id = OBJECT_ID(N'[dbo].[Stores]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_stores_number]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Stores] ADD  CONSTRAINT [DF_stores_number]  DEFAULT ((0.0)) FOR [number]
END


End
GO
/****** Object:  Default [DF_Users_uid]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Users_uid]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_uid]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_uid]  DEFAULT ([dbo].[getNewUid]()) FOR [uid]
END


End
GO
/****** Object:  Default [DF_Table_1_levels]    Script Date: 10/27/2015 16:54:25 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF_Table_1_levels]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Table_1_levels]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Table_1_levels]  DEFAULT ((1)) FOR [levels]
END


End
GO
/****** Object:  ForeignKey [FK_groupProductItem_Products]    Script Date: 10/27/2015 16:54:25 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_groupProductItem_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[groupProductItem]'))
ALTER TABLE [dbo].[groupProductItem]  WITH CHECK ADD  CONSTRAINT [FK_groupProductItem_Products] FOREIGN KEY([proid])
REFERENCES [dbo].[Products] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_groupProductItem_Products]') AND parent_object_id = OBJECT_ID(N'[dbo].[groupProductItem]'))
ALTER TABLE [dbo].[groupProductItem] CHECK CONSTRAINT [FK_groupProductItem_Products]
GO
/****** Object:  ForeignKey [FK_groupProductItem_proGroup]    Script Date: 10/27/2015 16:54:25 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_groupProductItem_proGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[groupProductItem]'))
ALTER TABLE [dbo].[groupProductItem]  WITH CHECK ADD  CONSTRAINT [FK_groupProductItem_proGroup] FOREIGN KEY([groupid])
REFERENCES [dbo].[proGroup] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_groupProductItem_proGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[groupProductItem]'))
ALTER TABLE [dbo].[groupProductItem] CHECK CONSTRAINT [FK_groupProductItem_proGroup]
GO
/****** Object:  ForeignKey [FK_proGroupItems_proCategory]    Script Date: 10/27/2015 16:54:25 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_proGroupItems_proCategory]') AND parent_object_id = OBJECT_ID(N'[dbo].[proGroupItems]'))
ALTER TABLE [dbo].[proGroupItems]  WITH CHECK ADD  CONSTRAINT [FK_proGroupItems_proCategory] FOREIGN KEY([categoryid])
REFERENCES [dbo].[proCategory] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_proGroupItems_proCategory]') AND parent_object_id = OBJECT_ID(N'[dbo].[proGroupItems]'))
ALTER TABLE [dbo].[proGroupItems] CHECK CONSTRAINT [FK_proGroupItems_proCategory]
GO
/****** Object:  ForeignKey [FK_proGroupItems_proGroup]    Script Date: 10/27/2015 16:54:25 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_proGroupItems_proGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[proGroupItems]'))
ALTER TABLE [dbo].[proGroupItems]  WITH CHECK ADD  CONSTRAINT [FK_proGroupItems_proGroup] FOREIGN KEY([groupid])
REFERENCES [dbo].[proGroup] ([id])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_proGroupItems_proGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[proGroupItems]'))
ALTER TABLE [dbo].[proGroupItems] CHECK CONSTRAINT [FK_proGroupItems_proGroup]
GO
