USE [uh419050_db]
GO
/****** Object:  Table [dbo].[ChatMessages]    Script Date: 03.09.2016 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatMessages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PublicKey] [uniqueidentifier] NOT NULL,
	[BelongToChat] [int] NULL,
	[IsNew] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[AttachmentMin] [nvarchar](200) NULL,
	[AttachmentMax] [nvarchar](200) NULL,
	[Owner] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Chats]    Script Date: 03.09.2016 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PublicKey] [uniqueidentifier] NOT NULL,
	[BelongsToUserA] [int] NULL,
	[BelongsToUserB] [int] NULL,
	[LastAction] [datetime] NOT NULL,
	[ExistNewMessage] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Events]    Script Date: 03.09.2016 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Decrtiption] [nvarchar](50) NOT NULL,
	[UserPk] [uniqueidentifier] NOT NULL,
	[Datetime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Mails]    Script Date: 03.09.2016 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Recipient] [nvarchar](50) NOT NULL,
	[EmailSubject] [nvarchar](50) NULL,
	[Datetime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Portfolios]    Script Date: 03.09.2016 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Portfolios](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BelongsToUser] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Description] [nvarchar](max) NULL,
	[Jobs] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tasks]    Script Date: 03.09.2016 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PublicKey] [uniqueidentifier] NOT NULL,
	[Customer] [int] NOT NULL,
	[Worker] [int] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[TopicalTo] [datetime] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Groop] [nvarchar](50) NULL,
	[SubGroop] [nvarchar](50) NULL,
	[Price] [int] NOT NULL,
	[ProccessStatus] [nvarchar](50) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[Closed] [datetime] NULL,
	[Coutry] [nvarchar](10) NULL,
	[City] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 03.09.2016 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PublicKey] [uniqueidentifier] NULL,
	[IsCompany] [bit] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[SurName] [nvarchar](20) NOT NULL,
	[PatronymicName] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](50) NOT NULL,
	[Phone1] [nvarchar](100) NULL,
	[Phone2] [nvarchar](100) NULL,
	[Phone3] [nvarchar](100) NULL,
	[Skype] [nvarchar](100) NULL,
	[Country] [nvarchar](20) NULL,
	[City] [nvarchar](20) NULL,
	[Ballance] [nvarchar](20) NULL,
	[Valuta] [nvarchar](20) NULL,
	[AccountCreated] [datetime] NOT NULL,
	[LastLogIn] [datetime] NULL,
	[UserLogoPath] [nvarchar](300) NULL,
	[Gender] [bit] NULL,
	[Birthday] [datetime] NULL,
	[Rating] [float] NULL,
	[Honors] [nvarchar](max) NULL,
	[SubscribeToGroups] [nvarchar](max) NULL,
	[IsEnabled] [bit] NOT NULL,
	[ReceiveNews] [bit] NOT NULL,
	[Lang] [nvarchar](50) NOT NULL,
	[IsAdmin] [bit] NULL,
	[IsModerator] [bit] NULL,
	[UserLogoPathMax] [nvarchar](300) NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[EmailDelivery] [bit] NULL DEFAULT ((0)),
	[SmsDelivery] [bit] NULL DEFAULT ((0)),
	[PushUpDelivery] [bit] NULL DEFAULT ((0)),
	[IsModerChecked] [bit] NOT NULL DEFAULT ((1)),
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[ChatMessages]  WITH CHECK ADD FOREIGN KEY([BelongToChat])
REFERENCES [dbo].[Chats] ([ID])
GO
ALTER TABLE [dbo].[Chats]  WITH CHECK ADD FOREIGN KEY([BelongsToUserA])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Chats]  WITH CHECK ADD FOREIGN KEY([BelongsToUserB])
REFERENCES [dbo].[Users] ([ID])
GO
