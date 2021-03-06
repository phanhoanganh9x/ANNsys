USE [inventorymanagement]
GO

/****** Object:  Table [dbo].[Video]    Script Date: 11/12/2020 5:11:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Video](
	[Id] [nvarchar](50) NOT NULL,
	[Thumbnail] [nvarchar](MAX) NOT NULL,
	[Url] [nvarchar](MAX) NOT NULL,
	[Height] [int] NOT NULL,
	[Width] [int] NOT NULL,
	[Expiration] [bigint] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL
 CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_Expiration]  DEFAULT (0) FOR [Expiration]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_CreatedBy]  DEFAULT (N'admin') FOR [CreatedBy]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_ModifiedBy]  DEFAULT (N'admin') FOR [ModifiedBy]
GO

ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

-- =============================================
-- Author:      Binh-TT
-- Create date: 2021-04-13
-- Description: Add the sub link download of video
-- ==========================================
ALTER TABLE [dbo].[Video] ADD [SubUrl] [nvarchar](MAX) NULL
GO