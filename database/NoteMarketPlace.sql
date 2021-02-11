USE [master]
GO
/****** Object:  Database [NoteMarketPlace]    Script Date: 11-02-2021 19:57:52 ******/
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
/****** Object:  Table [dbo].[Categories]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[id] [int] NOT NULL,
	[categoryName] [varchar](50) NOT NULL,
	[description] [varchar](max) NOT NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_noteCategory] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[countries]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[countries](
	[id] [int] NOT NULL,
	[countryName] [varchar](100) NOT NULL,
	[countryCode] [varchar](100) NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_noteCountry] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[downloads]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[downloads](
	[id] [int] NOT NULL,
	[seller] [int] NOT NULL,
	[noteID] [int] NOT NULL,
	[downloder] [int] NOT NULL,
	[isSellerHasAllowedDownload] [bit] NOT NULL,
	[attechmentPath] [varchar](max) NULL,
	[isAttechmentDownloadd] [bit] NOT NULL,
	[attechmentDownloadedDate] [datetime] NULL,
	[isPaid] [bit] NOT NULL,
	[purchasePrice] [decimal](18, 0) NULL,
	[noteTitle] [varchar](100) NOT NULL,
	[noteCategory] [varchar](100) NOT NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
 CONSTRAINT [PK_buyer] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteStatus]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteStatus](
	[id] [int] NOT NULL,
	[dataValue] [varchar](100) NOT NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[noteType]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[noteType](
	[id] [int] NOT NULL,
	[typeName] [varchar](100) NOT NULL,
	[description] [varchar](max) NOT NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_noteType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[refrenceGander]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[refrenceGander](
	[id] [int] NOT NULL,
	[datavalue] [varchar](100) NOT NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_refrenceGander] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sellerNoteReview]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sellerNoteReview](
	[id] [int] NOT NULL,
	[noteID] [int] NOT NULL,
	[reviewByID] [int] NOT NULL,
	[againstDownloadsID] [int] NOT NULL,
	[rating] [decimal](18, 0) NOT NULL,
	[comments] [varchar](max) NOT NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_reviewRating] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sellerNotes]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sellerNotes](
	[id] [int] NOT NULL,
	[sellerID] [int] NOT NULL,
	[status] [int] NOT NULL,
	[actionBy] [int] NULL,
	[adminRemark] [varchar](max) NULL,
	[publishedDate] [datetime] NULL,
	[type] [int] NOT NULL,
	[category] [int] NOT NULL,
	[country] [int] NOT NULL,
	[title] [varchar](50) NOT NULL,
	[picture] [varbinary](max) NULL,
	[university] [varchar](200) NULL,
	[descritption] [varchar](max) NOT NULL,
	[noPages] [int] NOT NULL,
	[course] [varchar](100) NULL,
	[courseCode] [varchar](100) NULL,
	[professorLecturer] [varchar](100) NULL,
	[Ispaid] [bit] NOT NULL,
	[sellPrice] [int] NOT NULL,
	[preview] [varbinary](max) NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_noteDetails_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sellerNotesAttechments]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sellerNotesAttechments](
	[id] [int] NOT NULL,
	[noteID] [int] NOT NULL,
	[fileName] [varchar](100) NOT NULL,
	[filePath] [varchar](max) NOT NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_seller] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sellerNotesReportedIssues]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sellerNotesReportedIssues](
	[id] [int] NOT NULL,
	[noteID] [int] NOT NULL,
	[reportedByID] [int] NOT NULL,
	[againstDownloadID] [int] NOT NULL,
	[remark] [varchar](max) NOT NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_inAppropriate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[sellType]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sellType](
	[id] [int] NOT NULL,
	[value] [bit] NOT NULL,
	[dataValue] [varchar](100) NOT NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[Isactive] [bit] NOT NULL,
 CONSTRAINT [PK_sellType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[systemConfiguration]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[systemConfiguration](
	[id] [int] NOT NULL,
	[key] [varchar](100) NOT NULL,
	[value] [varchar](max) NOT NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_systemConfiguration] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[id] [int] NOT NULL,
	[roleID] [int] NOT NULL,
	[firstName] [varchar](50) NOT NULL,
	[lastName] [varchar](50) NOT NULL,
	[emailID] [varchar](100) NOT NULL,
	[password] [varchar](24) NOT NULL,
	[IsEmailVarified] [bit] NOT NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK__user__3213E83F2275C61D] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[userProfile]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userProfile](
	[id] [int] NOT NULL,
	[userID] [int] NOT NULL,
	[gander] [int] NULL,
	[secondaryEmailAddress] [varchar](100) NULL,
	[DOB] [date] NULL,
	[phone-number-CountryCode] [varchar](5) NOT NULL,
	[phoneNumber] [int] NOT NULL,
	[college] [varchar](100) NULL,
	[university] [varchar](100) NULL,
	[country] [int] NOT NULL,
	[zipCode] [varchar](50) NOT NULL,
	[state] [varchar](50) NOT NULL,
	[city] [varchar](50) NOT NULL,
	[address01] [varchar](100) NOT NULL,
	[address02] [varchar](100) NOT NULL,
	[profilePicture] [varbinary](max) NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
 CONSTRAINT [PK_userProfile] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[userRole]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[userRole](
	[id] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[description] [varchar](max) NULL,
	[createdDate] [datetime] NULL,
	[createdBy] [int] NULL,
	[modifyDate] [datetime] NULL,
	[modifyBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_userRole] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[downloads]  WITH CHECK ADD  CONSTRAINT [FK_downloads_downloads_downloader] FOREIGN KEY([downloder])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[downloads] CHECK CONSTRAINT [FK_downloads_downloads_downloader]
GO
ALTER TABLE [dbo].[downloads]  WITH CHECK ADD  CONSTRAINT [FK_downloads_downloads_noteID] FOREIGN KEY([noteID])
REFERENCES [dbo].[sellerNotes] ([id])
GO
ALTER TABLE [dbo].[downloads] CHECK CONSTRAINT [FK_downloads_downloads_noteID]
GO
ALTER TABLE [dbo].[downloads]  WITH CHECK ADD  CONSTRAINT [FK_downloads_downloads_seller] FOREIGN KEY([seller])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[downloads] CHECK CONSTRAINT [FK_downloads_downloads_seller]
GO
ALTER TABLE [dbo].[sellerNoteReview]  WITH CHECK ADD  CONSTRAINT [FK_sellerNoteReview_downloads_downloadID] FOREIGN KEY([againstDownloadsID])
REFERENCES [dbo].[downloads] ([id])
GO
ALTER TABLE [dbo].[sellerNoteReview] CHECK CONSTRAINT [FK_sellerNoteReview_downloads_downloadID]
GO
ALTER TABLE [dbo].[sellerNoteReview]  WITH CHECK ADD  CONSTRAINT [FK_sellerNoteReview_sellerNotes] FOREIGN KEY([noteID])
REFERENCES [dbo].[sellerNotes] ([id])
GO
ALTER TABLE [dbo].[sellerNoteReview] CHECK CONSTRAINT [FK_sellerNoteReview_sellerNotes]
GO
ALTER TABLE [dbo].[sellerNoteReview]  WITH CHECK ADD  CONSTRAINT [FK_sellerNoteReview_user] FOREIGN KEY([reviewByID])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[sellerNoteReview] CHECK CONSTRAINT [FK_sellerNoteReview_user]
GO
ALTER TABLE [dbo].[sellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_sellerNotes_sellerNotes_ActionBy] FOREIGN KEY([actionBy])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[sellerNotes] CHECK CONSTRAINT [FK_sellerNotes_sellerNotes_ActionBy]
GO
ALTER TABLE [dbo].[sellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_sellerNotes_sellerNotes_Category] FOREIGN KEY([category])
REFERENCES [dbo].[Categories] ([id])
GO
ALTER TABLE [dbo].[sellerNotes] CHECK CONSTRAINT [FK_sellerNotes_sellerNotes_Category]
GO
ALTER TABLE [dbo].[sellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_sellerNotes_sellerNotes_country] FOREIGN KEY([country])
REFERENCES [dbo].[countries] ([id])
GO
ALTER TABLE [dbo].[sellerNotes] CHECK CONSTRAINT [FK_sellerNotes_sellerNotes_country]
GO
ALTER TABLE [dbo].[sellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_sellerNotes_sellerNotes_type] FOREIGN KEY([type])
REFERENCES [dbo].[noteType] ([id])
GO
ALTER TABLE [dbo].[sellerNotes] CHECK CONSTRAINT [FK_sellerNotes_sellerNotes_type]
GO
ALTER TABLE [dbo].[sellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_sellerNotes_status_noteStatus] FOREIGN KEY([status])
REFERENCES [dbo].[NoteStatus] ([id])
GO
ALTER TABLE [dbo].[sellerNotes] CHECK CONSTRAINT [FK_sellerNotes_status_noteStatus]
GO
ALTER TABLE [dbo].[sellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_sellerNotes_user] FOREIGN KEY([sellerID])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[sellerNotes] CHECK CONSTRAINT [FK_sellerNotes_user]
GO
ALTER TABLE [dbo].[sellerNotesAttechments]  WITH CHECK ADD  CONSTRAINT [FK_sellerNotesAttechments_sellerNotesAttechments_noteID] FOREIGN KEY([noteID])
REFERENCES [dbo].[sellerNotes] ([id])
GO
ALTER TABLE [dbo].[sellerNotesAttechments] CHECK CONSTRAINT [FK_sellerNotesAttechments_sellerNotesAttechments_noteID]
GO
ALTER TABLE [dbo].[sellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_sellerNotesReportedIssues_downloads_againstDownloadId] FOREIGN KEY([againstDownloadID])
REFERENCES [dbo].[downloads] ([id])
GO
ALTER TABLE [dbo].[sellerNotesReportedIssues] CHECK CONSTRAINT [FK_sellerNotesReportedIssues_downloads_againstDownloadId]
GO
ALTER TABLE [dbo].[sellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_sellerNotesReportedIssues_sellerNotes_noteID] FOREIGN KEY([noteID])
REFERENCES [dbo].[sellerNotes] ([id])
GO
ALTER TABLE [dbo].[sellerNotesReportedIssues] CHECK CONSTRAINT [FK_sellerNotesReportedIssues_sellerNotes_noteID]
GO
ALTER TABLE [dbo].[sellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_sellerNotesReportedIssues_user_reportedById] FOREIGN KEY([reportedByID])
REFERENCES [dbo].[user] ([id])
GO
ALTER TABLE [dbo].[sellerNotesReportedIssues] CHECK CONSTRAINT [FK_sellerNotesReportedIssues_user_reportedById]
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD  CONSTRAINT [FK_user_userRole] FOREIGN KEY([id])
REFERENCES [dbo].[userRole] ([id])
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT [FK_user_userRole]
GO
ALTER TABLE [dbo].[userProfile]  WITH CHECK ADD  CONSTRAINT [FK_userProfile_refrenceGander] FOREIGN KEY([id])
REFERENCES [dbo].[refrenceGander] ([id])
GO
ALTER TABLE [dbo].[userProfile] CHECK CONSTRAINT [FK_userProfile_refrenceGander]
GO
/****** Object:  StoredProcedure [dbo].[spSignup]    Script Date: 11-02-2021 19:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[spSignup]

@roleID_signup int,
@firstName_signup varchar(50),
@lastName_signup varchar(50),
@email_signup varchar(100),
@password_signup varchar(24),
@confirmPass_signup varchar(24),
@isvarified_signup bit
as
begin

	if exists(select roleID from [user] where emailID=@email_signup)
	begin
		print'this user already exist'
	end
	else
	begin
		IF(@confirmPass_signup=@password_signup)
		BEGIN
			declare @idforuser int
			select @idforuser = case when max(id) is null  then 0 else max(id) end 
			from [user]
			select @idforuser = @idforuser + 1
	
			insert into [user] values (@idforuser,@roleID_signup, @firstName_signup, @lastName_signup,  @email_signup, @password_signup, @isvarified_signup, getdate(), null, null, null, 'true')
		END
		else
		begin
			print 're-enter password'
		end
	end
	
end
GO
USE [master]
GO
ALTER DATABASE [NoteMarketPlace] SET  READ_WRITE 
GO
