USE [master]
GO
/****** Object:  Database [PipingInformationDB]    Script Date: 2016/4/24 21:53:36 ******/
CREATE DATABASE [PipingInformationDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PipingInformationDB', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\PipingInformationDB.mdf' , SIZE = 3328KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PipingInformationDB_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\PipingInformationDB_log.ldf' , SIZE = 3456KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [PipingInformationDB] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PipingInformationDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PipingInformationDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PipingInformationDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PipingInformationDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PipingInformationDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PipingInformationDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [PipingInformationDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [PipingInformationDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PipingInformationDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PipingInformationDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PipingInformationDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PipingInformationDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PipingInformationDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PipingInformationDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PipingInformationDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PipingInformationDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PipingInformationDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PipingInformationDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PipingInformationDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PipingInformationDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PipingInformationDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PipingInformationDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PipingInformationDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PipingInformationDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PipingInformationDB] SET  MULTI_USER 
GO
ALTER DATABASE [PipingInformationDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PipingInformationDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PipingInformationDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PipingInformationDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [PipingInformationDB] SET DELAYED_DURABILITY = DISABLED 
GO
USE [PipingInformationDB]
GO
/****** Object:  Table [dbo].[PipingDetectionInfo]    Script Date: 2016/4/24 21:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PipingDetectionInfo](
	[KeyID] [varchar](32) NOT NULL,
	[PipingID] [varchar](20) NULL,
	[VideoFile] [varchar](20) NULL,
	[StartWellNo] [varchar](20) NULL,
	[EndWellNo] [varchar](20) NULL,
	[LayingYear] [varchar](20) NULL,
	[StartPointDepth] [varchar](20) NULL,
	[EndPointDepth] [varchar](20) NULL,
	[TubulationType] [varchar](20) NULL,
	[TubulationMaterial] [varchar](20) NULL,
	[TubulationDiameter] [varchar](20) NULL,
	[DetectionDirect] [varchar](20) NULL,
	[TubulationLength] [varchar](20) NULL,
	[DetectionAddress] [varchar](50) NULL,
	[DetectionTime] [varchar](20) NULL,
	[AddTime] [datetime] NULL,
	[AddPerson] [varchar](20) NULL,
	[IsEnable] [tinyint] NULL,
	[IsDelete] [tinyint] NULL,
	[DetectionLength] [varchar](20) NULL,
	[DetectionFun] [varchar](20) NULL,
 CONSTRAINT [PK__PipingDe__21F5BE277F60ED59] PRIMARY KEY CLUSTERED 
(
	[KeyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PipingPictureInfo]    Script Date: 2016/4/24 21:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PipingPictureInfo](
	[KeyID] [varchar](32) NULL,
	[PipingID] [varchar](20) NULL,
	[PictureID] [varchar](20) NULL,
	[PictureFilePath] [varchar](100) NULL,
	[PipingInternalState] [varchar](150) NULL,
	[Grade] [varchar](20) NULL,
	[Score] [varchar](20) NULL,
	[DefectCode] [varchar](20) NULL,
	[Distance] [varchar](20) NULL,
	[AddTime] [datetime] NULL,
	[AddPerson] [varchar](20) NULL,
	[IsDelete] [tinyint] NULL,
	[IsEnable] [tinyint] NULL,
	[Remark] [varchar](200) NULL,
	[PictureType] [tinyint] NULL,
	[PictureRemark] [varchar](200) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 2016/4/24 21:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserInfo](
	[KeID] [varchar](32) NOT NULL,
	[UserName] [varchar](100) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[UserType] [int] NOT NULL,
	[TrueName] [varchar](100) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[ID] [varchar](20) NULL,
	[IsDelete] [int] NULL,
	[AuditDate] [datetime] NULL,
	[AuditUser] [varchar](100) NULL,
	[ApplyDate] [datetime] NULL,
	[UserID] [varchar](32) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[KeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
USE [master]
GO
ALTER DATABASE [PipingInformationDB] SET  READ_WRITE 
GO
