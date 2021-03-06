USE [master]
GO
/****** Object:  Database [NoteMarketPlace]    Script Date: 11-04-2021 13:48:14 ******/
CREATE DATABASE [NoteMarketPlace]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NoteMarketPlace', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\NoteMarketPlace.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NoteMarketPlace_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\NoteMarketPlace_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [NoteMarketPlace] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NoteMarketPlace].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NoteMarketPlace] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ARITHABORT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NoteMarketPlace] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NoteMarketPlace] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET  DISABLE_BROKER 
GO
ALTER DATABASE [NoteMarketPlace] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NoteMarketPlace] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET RECOVERY FULL 
GO
ALTER DATABASE [NoteMarketPlace] SET  MULTI_USER 
GO
ALTER DATABASE [NoteMarketPlace] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NoteMarketPlace] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NoteMarketPlace] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NoteMarketPlace] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'NoteMarketPlace', N'ON'
GO
ALTER DATABASE [NoteMarketPlace] SET QUERY_STORE = OFF
GO
USE [NoteMarketPlace]
GO
/****** Object:  Table [dbo].[AdminProfile]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminProfile](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[EmailID] [nvarchar](100) NOT NULL,
	[SecondryEmailID] [nvarchar](100) NULL,
	[PhoneNumber] [nvarchar](10) NULL,
	[PhoneNumberCode] [nvarchar](10) NULL,
	[ProfilePicture] [nvarchar](100) NULL,
 CONSTRAINT [PK_AdminProfile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CountryCode] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Downloads]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Downloads](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[Seller] [int] NOT NULL,
	[Downloader] [int] NOT NULL,
	[IsSellerHasAllowedDownload] [bit] NOT NULL,
	[AttechmentPath] [varchar](max) NULL,
	[IsAttechmentDownloaded] [bit] NOT NULL,
	[AttechmentDownloadedDate] [datetime] NULL,
	[IsPaid] [bit] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
	[NoteTitle] [varchar](100) NOT NULL,
	[NoteCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_Downloads] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[gender]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[gender](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[value] [varchar](6) NOT NULL,
 CONSTRAINT [PK_gender] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteCategories]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteCategories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteCategories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteStatus]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteStatus](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[value] [varchar](100) NOT NULL,
 CONSTRAINT [PK_NoteStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteTypes]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotes]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[SellerID] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ActionBy] [int] NULL,
	[AdminRemarks] [varchar](max) NULL,
	[PublishedDate] [datetime] NULL,
	[Title] [varchar](100) NOT NULL,
	[Category] [int] NOT NULL,
	[DisplayPicture] [varchar](500) NULL,
	[NoteType] [int] NULL,
	[NumberOfPages] [int] NULL,
	[Description] [varchar](max) NOT NULL,
	[UniversityName] [varchar](200) NULL,
	[Country] [int] NULL,
	[Course] [varchar](100) NULL,
	[CourseCode] [varchar](100) NULL,
	[Professor] [varchar](100) NULL,
	[IsPaid] [bit] NOT NULL,
	[SellingPrice] [decimal](18, 0) NULL,
	[NotesPreview] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesAttechments]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesAttechments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[FileName] [varchar](100) NOT NULL,
	[FilePath] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotesAttechments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReportedIssues]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReportedIssues](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReportedByID] [int] NOT NULL,
	[AgainstDownloadID] [int] NOT NULL,
	[Remarks] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_SellerNotesReportedIssues] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReviews]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReviews](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReviewedByID] [int] NOT NULL,
	[AgainstDownloadsID] [int] NOT NULL,
	[Ratings] [decimal](18, 0) NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotesReviews] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellingType]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellingType](
	[id] [int] NOT NULL,
	[value] [varchar](6) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemConfigurations]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfigurations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](100) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SystemConfigurations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[DOB] [datetime] NULL,
	[Gender] [int] NULL,
	[SecondryEmailAddress] [varchar](100) NULL,
	[PhoneNumber-code] [int] NULL,
	[PhoneNumber] [varchar](20) NULL,
	[ProfilePicture] [varchar](500) NULL,
	[AddressLine1] [varchar](100) NOT NULL,
	[AddressLine2] [varchar](100) NULL,
	[City] [varchar](50) NOT NULL,
	[State] [varchar](50) NOT NULL,
	[ZipCode] [varchar](50) NOT NULL,
	[Country] [int] NOT NULL,
	[University] [varchar](100) NULL,
	[College] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11-04-2021 13:48:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[EmailID] [varchar](100) NOT NULL,
	[Password] [varchar](24) NOT NULL,
	[IsEmailVerified] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 

INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'India', N'11', CAST(N'2021-04-01T00:00:00.000' AS DateTime), 32, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Australia', N'24', CAST(N'2021-12-17T00:00:00.000' AS DateTime), 32, CAST(N'2021-04-03T19:19:44.283' AS DateTime), 32, 0)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'United Kingdom', N'4', CAST(N'2021-12-17T00:00:00.000' AS DateTime), 32, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Canada', N'44', CAST(N'2021-02-17T00:00:00.000' AS DateTime), 32, CAST(N'2021-04-03T19:32:46.607' AS DateTime), 32, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'US', N'193', CAST(N'2021-03-17T00:00:00.000' AS DateTime), 32, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'Spain', N'87', CAST(N'2021-04-03T19:18:55.037' AS DateTime), 32, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, N'Nepal', N'65', CAST(N'2021-04-03T20:10:02.600' AS DateTime), 32, CAST(N'2021-04-03T20:10:15.413' AS DateTime), 32, 0)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, N'Europe', N'122', CAST(N'2021-04-09T19:48:16.743' AS DateTime), 44, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Downloads] ON 

INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (67, 126, 19, 27, 1, N'~/Membere/19/126/Attachment/', 1, CAST(N'2021-03-17T23:47:40.757' AS DateTime), 1, CAST(9 AS Decimal(18, 0)), N'XAMARIN.FORMS', N'Science', CAST(N'2021-03-17T23:39:02.600' AS DateTime), 27, CAST(N'2021-03-19T18:07:09.960' AS DateTime), 27)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (68, 127, 19, 27, 1, N'~/Membere/19/127/Attachment/', 1, CAST(N'2021-03-17T23:39:16.590' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Computer Operating System', N'Computer', CAST(N'2021-03-17T23:39:16.590' AS DateTime), 27, NULL, NULL)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (69, 127, 19, 18, 1, N'~/Membere/19/127/Attachment/', 1, CAST(N'2021-03-19T18:19:24.337' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Computer Operating System', N'Computer', CAST(N'2021-03-19T18:19:24.337' AS DateTime), 18, NULL, NULL)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (70, 126, 19, 31, 1, N'~/Membere/19/126/Attachment/', 1, CAST(N'2021-03-30T09:21:13.993' AS DateTime), 1, CAST(9 AS Decimal(18, 0)), N'XAMARIN.FORMS', N'Science', CAST(N'2021-03-30T09:06:53.687' AS DateTime), 31, CAST(N'2021-03-30T09:21:13.993' AS DateTime), 31)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (71, 127, 19, 31, 1, N'~/Membere/19/127/Attachment/', 1, CAST(N'2021-03-30T09:09:47.700' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Computer Operating System', N'Computer', CAST(N'2021-03-30T09:09:47.700' AS DateTime), 31, NULL, NULL)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (72, 136, 19, 31, 1, N'~/Membere/19/136/Attachment/', 1, CAST(N'2021-03-30T09:09:57.003' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'XML with C#', N'IT', CAST(N'2021-03-30T09:09:57.003' AS DateTime), 31, NULL, NULL)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (73, 146, 27, 31, 1, N'~/Membere/27/146/Attachment/', 1, CAST(N'2021-03-30T09:10:07.213' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Human Body', N'Science', CAST(N'2021-03-30T09:10:07.213' AS DateTime), 31, NULL, NULL)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (74, 127, 19, 24, 1, N'~/Membere/19/127/Attachment/', 1, CAST(N'2021-03-30T09:10:39.003' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Computer Operating System', N'Computer', CAST(N'2021-03-30T09:10:39.003' AS DateTime), 24, NULL, NULL)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (75, 143, 19, 24, 1, N'~/Membere/19/143/Attachment/', 1, CAST(N'2021-03-30T09:20:25.053' AS DateTime), 1, CAST(9 AS Decimal(18, 0)), N'Accounting and Management', N'Account', CAST(N'2021-03-30T09:10:46.813' AS DateTime), 24, CAST(N'2021-03-30T09:20:25.053' AS DateTime), 24)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (76, 146, 27, 24, 1, N'~/Membere/27/146/Attachment/', 1, CAST(N'2021-03-30T09:11:10.067' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Human Body', N'Science', CAST(N'2021-03-30T09:11:10.067' AS DateTime), 24, NULL, NULL)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (77, 126, 19, 24, 1, N'~/Membere/19/126/Attachment/', 1, CAST(N'2021-03-30T09:20:28.433' AS DateTime), 1, CAST(9 AS Decimal(18, 0)), N'XAMARIN.FORMS', N'Science', CAST(N'2021-03-30T09:11:18.550' AS DateTime), 24, CAST(N'2021-03-30T09:20:28.433' AS DateTime), 24)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (78, 126, 19, 23, 1, N'~/Membere/19/126/Attachment/', 1, CAST(N'2021-03-30T09:20:52.220' AS DateTime), 1, CAST(9 AS Decimal(18, 0)), N'XAMARIN.FORMS', N'Science', CAST(N'2021-03-30T09:12:12.127' AS DateTime), 23, CAST(N'2021-03-30T09:20:52.220' AS DateTime), 23)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (79, 127, 19, 23, 1, N'~/Membere/19/127/Attachment/', 1, CAST(N'2021-03-30T09:12:28.630' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Computer Operating System', N'Computer', CAST(N'2021-03-30T09:12:28.630' AS DateTime), 23, NULL, NULL)
INSERT [dbo].[Downloads] ([id], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttechmentPath], [IsAttechmentDownloaded], [AttechmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (80, 143, 19, 23, 1, N'~/Membere/19/143/Attachment/', 1, CAST(N'2021-03-30T09:20:55.203' AS DateTime), 1, CAST(9 AS Decimal(18, 0)), N'Accounting and Management', N'Account', CAST(N'2021-03-30T09:12:36.873' AS DateTime), 23, CAST(N'2021-03-30T09:20:55.203' AS DateTime), 23)
SET IDENTITY_INSERT [dbo].[Downloads] OFF
GO
SET IDENTITY_INSERT [dbo].[gender] ON 

INSERT [dbo].[gender] ([id], [value]) VALUES (1, N'Male')
INSERT [dbo].[gender] ([id], [value]) VALUES (2, N'female')
SET IDENTITY_INSERT [dbo].[gender] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteCategories] ON 

INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'IT', N'CE_books', CAST(N'2021-02-17T00:00:00.000' AS DateTime), 32, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'Computer', N'politics_books', CAST(N'2021-04-17T00:00:00.000' AS DateTime), 32, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Science', N'circuit and notwork', CAST(N'2021-02-17T00:00:00.000' AS DateTime), 32, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, N'History', N'Pencil art', CAST(N'2021-02-23T00:00:00.000' AS DateTime), 32, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, N'Account', N'Hydrolic suspention', CAST(N'2021-02-27T00:00:00.000' AS DateTime), 32, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (11, N'Electronix', N'circuitry related', CAST(N'2021-03-17T00:00:00.000' AS DateTime), 32, CAST(N'2021-04-03T11:25:20.870' AS DateTime), 32, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (12, N'Aeronautics', N'mechetronix', CAST(N'2021-02-17T00:00:00.000' AS DateTime), 32, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (13, N'MBA', N'managment', CAST(N'2021-04-03T11:27:22.020' AS DateTime), 32, CAST(N'2021-04-03T12:03:34.990' AS DateTime), 32, 0)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (14, N'Programming', N'Programming-desc', CAST(N'2021-04-09T19:44:22.810' AS DateTime), 32, CAST(N'2021-04-09T19:44:39.930' AS DateTime), 32, 0)
SET IDENTITY_INSERT [dbo].[NoteCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteStatus] ON 

INSERT [dbo].[NoteStatus] ([id], [value]) VALUES (1, N'Draft')
INSERT [dbo].[NoteStatus] ([id], [value]) VALUES (2, N'Submitted For Review')
INSERT [dbo].[NoteStatus] ([id], [value]) VALUES (3, N'In Review')
INSERT [dbo].[NoteStatus] ([id], [value]) VALUES (4, N'Published')
INSERT [dbo].[NoteStatus] ([id], [value]) VALUES (5, N'Rejected')
INSERT [dbo].[NoteStatus] ([id], [value]) VALUES (6, N'Removed')
SET IDENTITY_INSERT [dbo].[NoteStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteTypes] ON 

INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Hand Written', N'Lorem ipsum dolor sit amet', CAST(N'2021-02-02T00:00:00.000' AS DateTime), 32, CAST(N'2021-04-03T17:07:46.127' AS DateTime), 32, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'Ambition Edtion', N'Lorem ipsum dolor sit amet', CAST(N'2021-03-28T00:00:00.000' AS DateTime), 32, CAST(N'2021-04-03T17:06:57.273' AS DateTime), 32, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'used', N'Lorem ipsum dolor sit amet', CAST(N'2021-01-01T00:00:00.000' AS DateTime), 32, CAST(N'2021-04-03T17:07:59.233' AS DateTime), 32, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Story Book', N'Lorem ipsum dolor sit amet', CAST(N'2021-04-17T00:00:00.000' AS DateTime), 32, CAST(N'2021-04-03T17:07:30.623' AS DateTime), 32, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'University Notes', N'Lorem ipsum dolor sit amet', CAST(N'2021-03-17T00:00:00.000' AS DateTime), 32, CAST(N'2021-04-03T17:07:10.023' AS DateTime), 32, 0)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, N'Instruction Manual', N'Lorem ipsum dolor sit amet', CAST(N'2021-03-17T00:00:00.000' AS DateTime), 32, CAST(N'2021-04-03T17:07:38.717' AS DateTime), 32, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, N'Report', N'Lorem ipsum dolor sit amet', CAST(N'2021-04-03T17:09:05.253' AS DateTime), 32, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, N'Type', N'Type-desc', CAST(N'2021-04-09T19:45:05.560' AS DateTime), 32, CAST(N'2021-04-09T19:45:09.997' AS DateTime), 32, 0)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, N'small-book', N'small-book-desc', CAST(N'2021-04-09T19:47:21.360' AS DateTime), 44, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[NoteTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotes] ON 

INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (126, 19, 4, 32, NULL, CAST(N'2021-01-17T23:31:59.000' AS DateTime), N'XAMARIN.FORMS', 4, N'~/Membere/19/126/20210317232602266-2.jpg', NULL, 347, N'just for use in the Virtual Mechanics tutorials. More text. And more
text. And more text. And more text. And more text.
And', N'university of New York', 2, N'Networking', N'5676', N'Mr Max devidson', 1, CAST(9 AS Decimal(18, 0)), N'~/Membere/19/126/20210317232602296-A Sample PDF.pdf', CAST(N'2021-03-17T23:26:01.640' AS DateTime), 19, CAST(N'2021-03-17T23:31:59.433' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (127, 19, 4, 32, NULL, CAST(N'2021-04-17T23:31:59.000' AS DateTime), N'Computer Operating System', 3, N'~/Membere/19/127/20210317232743020-4.jpg', NULL, 199, N'just for use in the Virtual Mechanics tutorials. More text. And more
text. And more text. And more text. And more text.
And', N'University of Australia', 4, N'Computer Engineering', N'256', N'Mr Max devidson', 0, CAST(0 AS Decimal(18, 0)), N'~/Membere/19/127/20210317232743041-A Sample PDF.pdf', CAST(N'2021-03-17T23:27:43.010' AS DateTime), 19, CAST(N'2021-03-17T23:28:00.710' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (128, 19, 4, 32, N'not good', CAST(N'2021-03-31T17:08:48.670' AS DateTime), N'Essential WCF', 12, N'~/Membere/19/128/20210317233123785-5.jpg', NULL, 277, N'just for use in the Virtual Mechanics tutorials. More text. And more
text. And more text. And more text. And more text.
And', N'university of New York', 3, N'Networking', N'5676', N'Mr. Richard brown', 1, CAST(19 AS Decimal(18, 0)), N'~/Membere/19/128/20210317233123807-A Sample PDF.pdf', CAST(N'2021-03-17T23:31:23.770' AS DateTime), 19, CAST(N'2021-03-31T17:08:48.670' AS DateTime), 32, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (131, 19, 5, 32, N'not good', NULL, N'Essential WCF', 12, N'~/Membere/19/131/20210319150907914-1.jpg', NULL, 277, N'just for use in the Virtual Mechanics tutorials. More text. And more
text. And more text. And more text. And more text.
And', N'university of New York', 3, N'Networking', N'5676', N'Mr. Richard brown', 1, CAST(19 AS Decimal(18, 0)), N'~/Membere/19/131/20210317233123807-A Sample PDF.pdf', CAST(N'2021-03-18T01:25:21.647' AS DateTime), 19, CAST(N'2021-03-31T14:35:23.110' AS DateTime), 32, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (132, 19, 5, 32, N'please upload valid documents', NULL, N'Computer Operating System-Final Exam Book With Paper Solution', 3, N'~/Membere/19/132/20210320125906134-1.jpg', NULL, 137, N'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vitae erat nibh. Morbi imperdiet
scelerisque massa, non ornare turpis elementum consectetur. Praesent laoreet vitae libero eget', NULL, NULL, NULL, NULL, NULL, 1, CAST(5 AS Decimal(18, 0)), N'~/Membere/19/132/20210320125906148-A Sample PDF.pdf', CAST(N'2021-03-20T12:59:05.733' AS DateTime), 19, CAST(N'2021-03-31T12:01:00.357' AS DateTime), 32, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (133, 19, 4, 43, NULL, CAST(N'2021-04-05T16:37:43.467' AS DateTime), N'Basic computer Engineering', 1, N'~/Membere/19/133/20210320130639196-3.jpg', NULL, NULL, N'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vitae erat nibh. Morbi imperdiet
scelerisque massa, non ornare turpis elementum consectetur. Praesent laoreet vitae libero eget', NULL, NULL, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), N'~/Membere/19/133/20210320130639210-A Sample PDF.pdf', CAST(N'2021-03-20T13:06:39.187' AS DateTime), 19, CAST(N'2021-04-05T16:37:43.467' AS DateTime), 43, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (134, 19, 5, 32, N'still need improvment', NULL, N'Accounting', 10, N'~/Membere/19/134/20210320130934548-4.jpg', NULL, NULL, N'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vitae erat nibh. Morbi imperdiet
scelerisque massa, non ornare turpis elementum consectetur. Praesent laoreet vitae libero eget', NULL, NULL, NULL, NULL, NULL, 1, CAST(29 AS Decimal(18, 0)), N'~/Membere/19/134/20210320130934559-A Sample PDF.pdf', CAST(N'2021-03-20T13:09:34.543' AS DateTime), 19, CAST(N'2021-03-31T12:00:35.200' AS DateTime), 32, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (135, 19, 4, 32, N'need improvment', CAST(N'2021-03-31T17:20:15.520' AS DateTime), N'An Introduction To R', 3, N'~/Membere/19/135/20210320131249276-5.jpg', NULL, 199, N'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vitae erat nibh. Morbi imperdiet
scelerisque massa, non ornare turpis elementum consectetur. Praesent laoreet vitae libero eget', NULL, 1, N'Computer Engineering', N'256', N'Mr. Richard brown', 1, CAST(31 AS Decimal(18, 0)), N'~/Membere/19/135/20210320131437125-A Sample PDF.pdf', CAST(N'2021-03-20T13:12:49.270' AS DateTime), 19, CAST(N'2021-03-31T17:20:15.520' AS DateTime), 32, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (136, 19, 4, 32, NULL, CAST(N'2021-03-27T11:06:41.690' AS DateTime), N'XML with C#', 1, N'~/Membere/19/136/20210320131657384-3.jpg', NULL, 204, N'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vitae erat nibh. Morbi imperdiet
scelerisque massa, non ornare turpis elementum consectetur. Praesent laoreet vitae libero eget', NULL, 4, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2021-03-20T13:16:57.373' AS DateTime), 19, CAST(N'2021-03-27T11:06:41.123' AS DateTime), 32, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (140, 19, 5, 32, N'title is too large', NULL, N'Computer Operating System-Final Exam Book With System-Final Exam Book With Paper SolutionComputer ', 1, N'~/Membere/19/140/20210323072222682-6.jpg', NULL, 277, N'lorem Ipsum Need some more text lorem Ipsum Need some more textlorem Ipsum Need some more textlorem Ipsum Need some more textlorem Ipsum Need some more text', N'Hrvard', 1, N'Computer Engineering', N'5676', NULL, 1, CAST(9 AS Decimal(18, 0)), N'~/Membere/19/140/20210323072222703-A Sample PDF.pdf', CAST(N'2021-03-23T07:22:22.160' AS DateTime), 19, CAST(N'2021-03-31T17:12:18.007' AS DateTime), 32, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (141, 19, 6, 32, N'unnecessary book', CAST(N'2021-04-06T11:39:41.920' AS DateTime), N'JavaScript', 1, N'~/Membere/19/141/20210324150416022-3.jpg', NULL, 277, N'lorem ipsum neewe morewe texttt boxozit s needdenor thammdlorem ipsum neewe morewe texttt boxozit s needdenor thammdlorem ipsum neewe morewe texttt boxozit s needdenor thammd', N'University of Australia', 1, N'Networking', N'25675', NULL, 1, CAST(29 AS Decimal(18, 0)), N'~/Membere/19/141/20210324150416032-5.jpg', CAST(N'2021-03-24T15:04:15.777' AS DateTime), 19, CAST(N'2021-04-06T12:06:53.897' AS DateTime), 32, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (143, 19, 4, 32, NULL, CAST(N'2021-04-17T23:31:59.000' AS DateTime), N'Accounting and Management', 10, N'~/Membere/19/143/20210326130959674-1.jpg', NULL, 204, N'lorem ipsum needdefs mowre textes giusd lorem ipsum needdefs mowre textes giusdlorem ipsum needdefs mowre textes giusdlorem ipsum needdefs mowre textes giusd', N'University of California', 1, N'data science', N'567676', N'Mr. Richard brown', 1, CAST(9 AS Decimal(18, 0)), N'~/Membere/19/143/20210326130959698-sample.pdf', CAST(N'2021-03-26T13:09:59.150' AS DateTime), 19, CAST(N'2021-03-26T13:10:15.007' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (144, 19, 6, 32, N'book should not have duplicate content', CAST(N'2021-04-17T23:31:59.000' AS DateTime), N'Introduction To R', 1, NULL, NULL, NULL, N'lorem ipsum nedwed morews textkb lorem ipsum nedwed morews textkb lorem ipsum nedwed morews textkb lorem ipsum nedwed morews textkb lorem ipsum nedwed morews textkb ', NULL, NULL, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2021-03-26T14:35:31.053' AS DateTime), 19, CAST(N'2021-03-28T18:01:38.003' AS DateTime), 32, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (145, 19, 1, NULL, NULL, NULL, N'DBMS', 11, NULL, NULL, NULL, N'lorem ipsum tetxet neddredlorem ipsum tetxet neddredlorem ipsum tetxet neddredlorem ipsum tetxet neddredlorem ipsum tetxet neddredlorem ipsum tetxet neddred ', NULL, NULL, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2021-03-26T16:30:41.693' AS DateTime), 19, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (146, 27, 4, 32, NULL, CAST(N'2021-03-27T20:41:58.063' AS DateTime), N'Human Body', 4, N'~/Membere/27/146/20210327113959898-4.jpg', NULL, 204, N'lorem ipsum soem txetlorem ipsum lorem ipsum soem txetlorem ipsum soem txetlorem ipsum soem txetlorem ipsum soem txetlorem ipsum soem txet', NULL, 4, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2021-03-27T11:39:59.447' AS DateTime), 27, CAST(N'2021-03-27T20:41:58.063' AS DateTime), 32, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (147, 19, 1, NULL, NULL, NULL, N'Power Electronics', 11, N'~/Membere/19/147/20210408134550062-4.jpg', NULL, 277, N'Lorem Ipsum doret init Lorem Ipsum doret init Lorem Ipsum doret init Lorem Ipsum doret init Lorem Ipsum doret init Lorem Ipsum doret init Lorem Ipsum doret init Lorem Ipsum doret init ', N'university of New York', 1, N'Computer Engineering', N'25675', NULL, 1, CAST(17 AS Decimal(18, 0)), N'~/Membere/19/147/20210408134550078-alterPdf.pdf', CAST(N'2021-04-08T13:45:49.640' AS DateTime), 19, CAST(N'2021-04-09T18:14:14.187' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (148, 19, 4, 32, NULL, CAST(N'2021-04-08T19:10:53.357' AS DateTime), N'Java', 1, NULL, NULL, NULL, N'Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum ', NULL, NULL, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2021-04-08T18:58:25.623' AS DateTime), 19, CAST(N'2021-04-08T19:10:53.357' AS DateTime), 32, 1)
INSERT [dbo].[SellerNotes] ([id], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberOfPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (153, 19, 2, NULL, NULL, NULL, N'javascript First edition', 3, NULL, NULL, NULL, N'lorem ipsum lorem ipsum lorem ipsum lorem ipsum lorem ipsum lorem ipsum lorem ipsum lorem ipsum lorem ipsum lorem ipsum lorem ipsum lorem ipsum lorem ipsum lorem ipsum lorem ipsum ', NULL, NULL, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2021-04-09T17:57:14.733' AS DateTime), 19, CAST(N'2021-04-09T18:01:20.273' AS DateTime), 19, 1)
SET IDENTITY_INSERT [dbo].[SellerNotes] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesAttechments] ON 

INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (113, 126, N'20210317232602493-smallPdf.pdf', N'~/Membere/19/126/Attachment/', CAST(N'2021-03-17T23:26:02.520' AS DateTime), 19, CAST(N'2021-03-17T23:31:59.437' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (114, 127, N'20210317232743058-alterPdf.pdf', N'~/Membere/19/127/Attachment/', CAST(N'2021-03-17T23:27:43.097' AS DateTime), 19, CAST(N'2021-03-17T23:28:00.713' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (115, 128, N'20210317233123877-smallPdf.pdf', N'~/Membere/19/128/Attachment/', CAST(N'2021-03-17T23:31:23.903' AS DateTime), 19, CAST(N'2021-03-17T23:31:48.293' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (117, 131, N'20210319150931056-alterPdf.pdf', N'~/Membere/19/131/Attachment/', CAST(N'2021-03-18T01:27:25.337' AS DateTime), 19, CAST(N'2021-03-24T15:05:51.640' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (118, 132, N'20210320125906249-smallPdf.pdf', N'~/Membere/19/132/Attachment/', CAST(N'2021-03-20T12:59:06.267' AS DateTime), 19, CAST(N'2021-03-24T15:05:39.400' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (119, 133, N'20210320130639220-alterPdf.pdf', N'~/Membere/19/133/Attachment/', CAST(N'2021-03-20T13:06:39.240' AS DateTime), 19, CAST(N'2021-03-24T15:05:24.063' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (120, 134, N'20210320130934571-alterPdf.pdf', N'~/Membere/19/134/Attachment/', CAST(N'2021-03-20T13:09:34.597' AS DateTime), 19, CAST(N'2021-03-24T15:05:13.473' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (121, 135, N'20210320131249330-dummy.pdf', N'~/Membere/19/135/Attachment/', CAST(N'2021-03-20T13:12:49.343' AS DateTime), 19, CAST(N'2021-03-24T15:05:06.023' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (122, 136, N'20210320131657437-dummy.pdf', N'~/Membere/19/136/Attachment/', CAST(N'2021-03-20T13:16:57.447' AS DateTime), 19, CAST(N'2021-03-24T15:04:59.173' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (123, 140, N'20210323072222819-smallPdf.pdf', N'~/Membere/19/140/Attachment/', CAST(N'2021-03-23T07:22:22.853' AS DateTime), 19, CAST(N'2021-03-23T07:23:27.137' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (124, 141, N'20210324150416125-dummy.pdf', N'~/Membere/19/141/Attachment/', CAST(N'2021-03-24T15:04:16.143' AS DateTime), 19, CAST(N'2021-03-24T15:04:50.063' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (125, 143, N'20210326130959881-dummy.pdf', N'~/Membere/19/143/Attachment/', CAST(N'2021-03-26T13:09:59.913' AS DateTime), 19, CAST(N'2021-03-26T13:10:15.007' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (126, 144, N'20210326143531638-smallPdf.pdf', N'~/Membere/19/144/Attachment/', CAST(N'2021-03-26T14:35:31.677' AS DateTime), 19, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (127, 145, N'20210326163042371-dummy.pdf', N'~/Membere/19/145/Attachment/', CAST(N'2021-03-26T16:30:42.400' AS DateTime), 19, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (128, 146, N'20210327113959969-smallPdf.pdf', N'~/Membere/27/146/Attachment/', CAST(N'2021-03-27T11:40:00.013' AS DateTime), 27, CAST(N'2021-03-27T11:40:28.120' AS DateTime), 27, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (129, 147, N'20210408134550317-smallPdf.pdf', N'~/Membere/19/147/Attachment/', CAST(N'2021-04-08T13:45:50.337' AS DateTime), 19, CAST(N'2021-04-09T18:14:14.187' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (130, 148, N'20210408185826157-alterPdf.pdf', N'~/Membere/19/148/Attachment/', CAST(N'2021-04-08T18:58:26.247' AS DateTime), 19, CAST(N'2021-04-08T19:00:36.870' AS DateTime), 19, 1)
INSERT [dbo].[SellerNotesAttechments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (135, 153, N'20210409175715206-alterPdf.pdf', N'~/Membere/19/153/Attachment/', CAST(N'2021-04-09T17:57:15.287' AS DateTime), 19, CAST(N'2021-04-09T18:01:20.277' AS DateTime), 19, 1)
SET IDENTITY_INSERT [dbo].[SellerNotesAttechments] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesReviews] ON 

INSERT [dbo].[SellerNotesReviews] ([id], [NoteID], [ReviewedByID], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, 126, 27, 67, CAST(5 AS Decimal(18, 0)), N'all the concept are clear', CAST(N'2021-03-17T23:46:09.407' AS DateTime), 27, CAST(N'2021-03-19T18:07:49.163' AS DateTime), 27, 1)
SET IDENTITY_INSERT [dbo].[SellerNotesReviews] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemConfigurations] ON 

INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Semail', N'xyz@gmail.com', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'ScontactNo', N'9876556789', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'Aemail', N'xyz@gmail.com', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'DefaultNoteDisplayPicture', N'~/Membere/system/dfBookPic/BookImg.png', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, N'DefaultProfilePicture', N'~/Membere/system/dfProfilePic/noUser.jpg', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (12, N'Facebook', N'https://www.google.com/search?q=facebook&rlz=1C1CHZN_enIN940IN940&oq=fac&aqs=chrome.0.0i131i433j69i57j0i131i433j0i433l2j0i131i433j0j0i131i433j0i433j0i131i433.1762j0j15&sourceid=chrome&ie=UTF-8', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (14, N'twitter', N'https://www.google.com/search?q=twitter&rlz=1C1CHZN_enIN940IN940&ei=Vd5lYI_RM56b4-EPyOK14AI&oq=twitter&gs_lcp=Cgdnd3Mtd2l6EAMyCggAELEDEIMBEEMyCggAELEDEIMBEEMyCggAELEDEIMBEEMyCAgAELEDEIMBMgsIABCxAxCDARDJAzIFCAAQkgMyBQgAEJIDMgIIADIICAAQsQMQgwEyBQgAELEDOgcIABBHELADOgcIABCwAxBDOgoIABDqAhC0AhBDOggIABDqAhCPAToHCAAQsQMQQzoECAAQQzoHCC4QsQMQQzoFCC4QsQNQn9IBWO7cAWDA5QFoAnACeACAAdMBiAGIC5IBBTAuNy4xmAEAoAEBqgEHZ3dzLXdperABCsgBCsABAQ&sclient=gws-wiz&ved=0ahUKEwjPz8Wept3vAhWezTgGHUhxDSwQ4dUDCA0&uact=5', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (15, N'Linkdin', N'https://www.google.com/search?q=linkedin&rlz=1C1CHZN_enIN940IN940&oq=linkdin&aqs=chrome.1.69i57j0i10j0i10i433j0i10j0i10i433i457j0i10l2j0i10i433j0i10i131i433j0i10i433.2292j0j9&sourceid=chrome&ie=UTF-8', NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[SystemConfigurations] OFF
GO
SET IDENTITY_INSERT [dbo].[UserProfile] ON 

INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondryEmailAddress], [PhoneNumber-code], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, 19, NULL, 1, NULL, 11, N'9874534587', N'~/Membere/19/20210312120113394-team-2.jpg', N'xyz street, xyz area, xyz road, xyz city, xyz', N'xyz street.', N'banglore', N'shouth west xyz', N'xyz00034', 3, N'international xyz xyz ', N'xyz', CAST(N'2021-03-12T12:01:22.433' AS DateTime), 19, CAST(N'2021-04-09T19:54:16.643' AS DateTime), 19)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondryEmailAddress], [PhoneNumber-code], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, 28, CAST(N'2021-03-27T00:00:00.000' AS DateTime), 1, NULL, 41, N'9874534587', N'~/Membere/28/20210313112727764-team-4.jpg', N'xyz street, xyz area, xyz road, xyz city, xyz', NULL, N'banglore', N'shouth west xyz', N'xyz00034', 5, N'international us university', NULL, CAST(N'2021-03-13T11:27:27.793' AS DateTime), 28, NULL, NULL)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondryEmailAddress], [PhoneNumber-code], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, 21, CAST(N'1999-03-19T00:00:00.000' AS DateTime), 2, NULL, 193, N'9874534345', N'~/Membere/21/20210316201530428-team-5.jpg', N'xyz street, xyz area, xyz road, xyz city, xyz', NULL, N'banglore', N'shouth west xyz', N'xyz00034', 3, NULL, NULL, CAST(N'2021-03-16T20:15:30.440' AS DateTime), 21, NULL, NULL)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondryEmailAddress], [PhoneNumber-code], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, 22, CAST(N'1971-09-14T00:00:00.000' AS DateTime), 2, NULL, 24, N'9876313244', N'~/Membere/22/20210316201658206-team-3.jpg', N'xyz street, xyz area, xyz road, xyz city, xyz', NULL, N'banglore', N'shouth west xyz', N'xyz00034', 4, NULL, NULL, CAST(N'2021-03-16T20:16:58.217' AS DateTime), 22, NULL, NULL)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondryEmailAddress], [PhoneNumber-code], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, 24, CAST(N'1994-12-20T00:00:00.000' AS DateTime), 2, NULL, 11, N'9884599999', N'~/Membere/24/20210316201821410-team-3.jpg', N'xyz street, xyz area, xyz road, xyz city, xyz', NULL, N'banglore', N'shouth west xyz', N'xyz00034', 5, NULL, NULL, CAST(N'2021-03-16T20:18:21.427' AS DateTime), 24, NULL, NULL)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondryEmailAddress], [PhoneNumber-code], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (6, 27, CAST(N'2004-10-27T00:00:00.000' AS DateTime), 1, NULL, 41, N'9898727845', N'~/Membere/27/20210316203559779-team-2.jpg', N'xyz street, xyz area, xyz road, xyz city, xyz', NULL, N'banglore', N'shouth west xyz', N'xyz00034', 5, NULL, NULL, CAST(N'2021-03-16T20:35:59.790' AS DateTime), 27, NULL, NULL)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondryEmailAddress], [PhoneNumber-code], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (7, 31, CAST(N'2000-03-27T00:00:00.000' AS DateTime), NULL, NULL, 41, N'9384601926', N'~/Membere/31/20210321102013479-customer-4.png', N'xyz street, xyz area, xyz road, xyz city, xyz', NULL, N'banglore', N'shouth west xyz', N'xyz00034', 3, NULL, NULL, CAST(N'2021-03-21T09:49:12.153' AS DateTime), 31, CAST(N'2021-03-21T10:20:13.480' AS DateTime), 31)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondryEmailAddress], [PhoneNumber-code], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (8, 42, NULL, NULL, NULL, NULL, NULL, NULL, N'NA', N'NA', N'NA', N'NA', N'NA', 11, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondryEmailAddress], [PhoneNumber-code], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (9, 43, NULL, NULL, NULL, NULL, NULL, N'~/Membere/43/team-4.jpg', N'NA', N'NA', N'NA', N'NA', N'NA', 11, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondryEmailAddress], [PhoneNumber-code], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (10, 44, NULL, NULL, NULL, 44, N'9865239067', N'~/Membere/44/team-3.jpg', N'NA', N'NA', N'NA', N'NA', N'NA', 11, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 

INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Super Admin', N'super admindesc', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'admin', N'admin desc', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'user', N'user desc', NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, 3, N'nihar', N'somal', N'nihar@gmail.com', N'123@123', NULL, CAST(N'2021-02-26T10:47:25.260' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, 3, N'jiraj', N'mehta', N'jiraj@gmail.com', N'8sGhJ', NULL, CAST(N'2021-02-26T14:26:52.877' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (17, 3, N'rahul', N'dravid', N'dravid@gmail.com', N'Hiren@123', NULL, CAST(N'2021-03-02T13:56:50.343' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (18, 3, N'poonam', N'nohara', N'poonam@gmail.com', N'Hiren@123', NULL, CAST(N'2021-03-02T13:57:24.263' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (19, 3, N'sinchan', N'nohara', N'sinchan@gmail.com', N'Hiren@123', 1, CAST(N'2021-03-02T13:57:47.477' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (20, 3, N'samoni', N'hirika', N'samoni@gmail.com', N'Hiren@123', NULL, CAST(N'2021-03-02T13:58:44.437' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (21, 3, N'riya', N'michell', N'riya@gmail.com', N'Hiren@123', NULL, CAST(N'2021-03-02T13:59:17.727' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (22, 3, N'rubina', N'nehval', N'rubina@gmail.com', N'Hiren@123', NULL, CAST(N'2021-03-02T13:59:48.617' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (23, 3, N'sanjay', N'dutt', N'sanjay@gmail.com', N'Hiren@123', 1, CAST(N'2021-03-02T14:00:11.697' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (24, 3, N'rushi', N'chandarana', N'rushi@gmail.com', N'Hiren@123', 1, CAST(N'2021-03-02T14:00:36.313' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (25, 3, N'ramesh', N'parekh', N'ramesh@gmail.com', N'Hiren@123', 1, CAST(N'2021-03-07T09:08:19.490' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (26, 3, N'rakesh', N'channiwala', N'rakesh@gmail.com', N'Hiren@123', 1, CAST(N'2021-03-07T09:12:48.190' AS DateTime), NULL, CAST(N'2021-04-01T15:11:20.037' AS DateTime), 32, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (27, 3, N'suhas', N'bajaj', N'suhas@gmail.com', N'Hiren@123', 1, CAST(N'2021-03-07T15:02:52.060' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (28, 3, N'richard', N'nailson', N'richard@gmail.com', N'Hiren@123', 1, CAST(N'2021-03-12T15:41:18.420' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (31, 3, N'dev', N'gadot', N'hirenpithva150@gmail.com', N'Hiren@123', 1, CAST(N'2021-03-21T09:44:05.523' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (32, 1, N'dhiraj', N'saksena', N'dhiraj@gmail.com', N'Hiren@123', 1, CAST(N'2021-03-23T17:46:37.127' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (42, 2, N'preato', N'escoza', N'preato@gmail.com', N'preato@123', 1, CAST(N'2021-04-05T10:57:07.903' AS DateTime), 32, CAST(N'2021-04-05T11:25:03.313' AS DateTime), 32, 0)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (43, 2, N'nikunaj', N'mehta', N'nikunaj@gmail.com', N'nikunaj@123', 1, CAST(N'2021-04-05T14:08:52.977' AS DateTime), 32, NULL, NULL, 1)
INSERT [dbo].[Users] ([id], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (44, 2, N'ravaniya', N'readdy', N'Ravaniya@gmail.com', N'ravaniya@123', 1, CAST(N'2021-04-09T19:42:31.707' AS DateTime), 32, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Countries] ADD  CONSTRAINT [DF_Countries_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NoteCategories] ADD  CONSTRAINT [DF_isActive_NoteCategory]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NoteTypes] ADD  CONSTRAINT [DF_NoteTypes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotes] ADD  CONSTRAINT [DF_SellerNotes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotesAttechments] ADD  CONSTRAINT [DF_SellerNotesAttechments_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotesReviews] ADD  CONSTRAINT [DF_SellerNotesReviews_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [DF_UserRoles_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [df_isActive_users]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AdminProfile]  WITH CHECK ADD  CONSTRAINT [FK_AdminProfile_UserID_Users_id] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[AdminProfile] CHECK CONSTRAINT [FK_AdminProfile_UserID_Users_id]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_donwloader_Users_id] FOREIGN KEY([Downloader])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_donwloader_Users_id]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_seller_Users_id] FOREIGN KEY([Seller])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_seller_Users_id]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_ActionBy_Users_id] FOREIGN KEY([ActionBy])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_ActionBy_Users_id]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_category_NoteCategories_id] FOREIGN KEY([Category])
REFERENCES [dbo].[NoteCategories] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_category_NoteCategories_id]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Country_Countries_id] FOREIGN KEY([Country])
REFERENCES [dbo].[Countries] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Country_Countries_id]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_NoteType_NoteTypes_id] FOREIGN KEY([NoteType])
REFERENCES [dbo].[NoteTypes] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_NoteType_NoteTypes_id]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_sellerID_Users_id] FOREIGN KEY([SellerID])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_sellerID_Users_id]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_status_NoteStatus_id] FOREIGN KEY([Status])
REFERENCES [dbo].[NoteStatus] ([id])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_status_NoteStatus_id]
GO
ALTER TABLE [dbo].[SellerNotesAttechments]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesAttechments_NoteID_SellerNotes_id] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([id])
GO
ALTER TABLE [dbo].[SellerNotesAttechments] CHECK CONSTRAINT [FK_SellerNotesAttechments_NoteID_SellerNotes_id]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_againstDownloadID_NOteID_Downloads_id] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([id])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_againstDownloadID_NOteID_Downloads_id]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_NoteID_SellerNotes_id] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([id])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_NoteID_SellerNotes_id]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_ReportedByID_Users_id] FOREIGN KEY([ReportedByID])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_ReportedByID_Users_id]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_RoleID_UserRoles_id] FOREIGN KEY([RoleID])
REFERENCES [dbo].[UserRoles] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_RoleID_UserRoles_id]
GO
USE [master]
GO
ALTER DATABASE [NoteMarketPlace] SET  READ_WRITE 
GO
