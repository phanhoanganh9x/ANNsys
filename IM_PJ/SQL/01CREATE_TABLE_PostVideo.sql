USE [inventorymanagement]
GO

/****** Object:  Table [dbo].[PostVideo]    Script Date: 11/12/2020 5:11:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PostVideo](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [VideoId] [nvarchar](50) NOT NULL,
    [PostId] [int] NULL,
    [IsActive] [bit] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL
 CONSTRAINT [PK_PostVideo] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PostVideo] ADD  CONSTRAINT [DF_PostVideo_IsActive]  DEFAULT (0) FOR [IsActive]
GO

ALTER TABLE [dbo].[PostVideo] ADD  CONSTRAINT [DF_PostVideo_CreatedBy]  DEFAULT (N'admin') FOR [CreatedBy]
GO

ALTER TABLE [dbo].[PostVideo] ADD  CONSTRAINT [DF_PostVideo_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[PostVideo] ADD  CONSTRAINT [DF_PostVideo_ModifiedBy]  DEFAULT (N'admin') FOR [ModifiedBy]
GO

ALTER TABLE [dbo].[PostVideo] ADD  CONSTRAINT [DF_PostVideo_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

ALTER TABLE [dbo].[PostVideo]  WITH CHECK ADD  CONSTRAINT [FK_PostVideo_Video] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Video] ([Id])
GO

ALTER TABLE [dbo].[PostVideo] CHECK CONSTRAINT [FK_PostVideo_Video]
GO

ALTER TABLE [dbo].[PostVideo]  WITH CHECK ADD  CONSTRAINT [FK_PostVideo_PostPublic] FOREIGN KEY([PostId])
REFERENCES [dbo].[PostPublic] ([Id])
GO

ALTER TABLE [dbo].[PostVideo] CHECK CONSTRAINT [FK_PostVideo_PostPublic]
GO

ALTER TABLE [dbo].[PostVideo] ADD  CONSTRAINT [UK_PostVideo] UNIQUE NONCLUSTERED
(
    [VideoId] ASC,
    [PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO