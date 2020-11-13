USE [inventorymanagement]
GO

/****** Object:  Table [dbo].[Video]    Script Date: 11/12/2020 5:11:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Video](
	[VideoId] [nvarchar](50) NOT NULL,
	[Url] [nvarchar](MAX) NULL,
	[Thumbnail] [nvarchar](MAX) NULL,
	[Expiration] [timestamp] NULL,
	[IsPublicVideo] AS IIF(ISNULL([IsProductVideo], 0) = 0 and ISNULL([IsPostVideo], 0) = 0, 1, 0),
	[IsProductVideo] [bit] NOT NULL,
	[ProductId] [int] NULL,
	[IsPostVideo] [bit] NOT NULL,
	[PostId] [int] NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED 
(
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_Expiration]  DEFAULT (getdate()) FOR [Expiration]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_IsProductVideo]  DEFAULT (0) FOR [IsProductVideo]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_IsPostVideo]  DEFAULT (0) FOR [IsPostVideo]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_CreatedBy]  DEFAULT (N'admin') FOR [CreatedBy]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_ModifiedBy]  DEFAULT (N'admin') FOR [ModifiedBy]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
