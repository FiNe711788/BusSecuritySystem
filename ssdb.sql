USE [SafeSystem]
GO

/****** Object:  Table [dbo].[CriminalPicture]    Script Date: 12/02/2015 19:00:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CriminalPicture](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CriminalLiveTemplate] [varbinary](max) NOT NULL,
	[CriminalPicture] [nvarchar](max) NOT NULL,
	[Time] [datetime] NOT NULL,
 CONSTRAINT [PK_CriminalPicture] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [SafeSystem]
GO

/****** Object:  Table [dbo].[CriminalRecognition]    Script Date: 12/02/2015 19:01:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CriminalRecognition](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Template] [varbinary](max) NOT NULL,
	[Time] [datetime] NOT NULL,
 CONSTRAINT [PK_CriminalRecognition] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
USE [SafeSystem]
GO

/****** Object:  Table [dbo].[FaceRecognition]    Script Date: 12/02/2015 19:02:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FaceRecognition](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Template] [varbinary](max) NOT NULL,
	[ShangCheShiJian] [datetime] NULL,
	[XiaCheShiJian] [datetime] NULL,
 CONSTRAINT [PK_FaceRecognition] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [SafeSystem]
GO

/****** Object:  Table [dbo].[GPS]    Script Date: 12/02/2015 19:03:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GPS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Longitude] [nchar](10) NULL,
	[Latitude] [nchar](10) NULL,
	[DateTime] [datetime] NULL
) ON [PRIMARY]

GO


USE [SafeSystem]
GO

/****** Object:  Table [dbo].[NumberOfPeople]    Script Date: 12/02/2015 19:03:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NumberOfPeople](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NumberOfPeople] [int] NOT NULL,
	[Time] [datetime] NOT NULL,
 CONSTRAINT [PK_NumberOfPeople] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [SafeSystem]
GO

/****** Object:  Table [dbo].[Serial]    Script Date: 2015/12/2 19:30:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Serial](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Alcohol] [int] NULL,
	[Smoke] [int] NULL,
	[Petrol] [int] NULL,
	[temper] [float] NULL,
	[time] [datetime] NULL,
 CONSTRAINT [PK_Serial] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO




