USE [inventorymanagement]
GO

/****** Object:  Table [dbo].[Video]    Script Date: 11/12/2020 5:11:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Video](
	[Id] [nvarchar](50) NOT NULL,
	[Thumbnail] [nvarchar](MAX) NULL,
	[Url] [nvarchar](MAX) NULL,
	[Height] [int] NULL,
	[Width] [int] NULL,
	[Expiration] [bigint] NOT NULL,
	[IsPublicVideo] AS IIF(IsProductVideo = 0 and IsPostVideo = 0, CAST(1 as BIT), CAST(0 AS BIT)),
	[IsProductVideo] [bit] NOT NULL,
	[ProductId] [int] NULL,
	[IsPostVideo] [bit] NOT NULL,
	[PostId] [int] NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[Timestamp] [timestamp] NOT NULL
 CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_Expiration]  DEFAULT (0) FOR [Expiration]
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


DELETE FROM [VIDEO]

INSERT INTO [Video]([ID], [IsProductVideo], [IsPostVideo]) VALUES (N'5yP6pq8IMOw', 0, 0)
INSERT INTO [Video]([ID], [IsProductVideo], [IsPostVideo]) VALUES (N'9xwazD5SyVg', 0, 1)
INSERT INTO [Video]([ID], [IsProductVideo], [IsPostVideo]) VALUES (N'3EBG0NmfnUw', 1, 0)