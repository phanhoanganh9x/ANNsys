USE [inventorymanagement]
GO

/****** Object:  Table [dbo].[ProductVideo]    Script Date: 11/12/2020 5:11:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProductVideo](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [VideoId] [nvarchar](50) NOT NULL,
    [ProductId] [int] NULL,
    [IsActive] [bit] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL
 CONSTRAINT [PK_ProductVideo] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProductVideo] ADD  CONSTRAINT [DF_ProductVideo_IsActive]  DEFAULT (0) FOR [IsActive]
GO

ALTER TABLE [dbo].[ProductVideo] ADD  CONSTRAINT [DF_ProductVideo_CreatedBy]  DEFAULT (N'admin') FOR [CreatedBy]
GO

ALTER TABLE [dbo].[ProductVideo] ADD  CONSTRAINT [DF_ProductVideo_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[ProductVideo] ADD  CONSTRAINT [DF_ProductVideo_ModifiedBy]  DEFAULT (N'admin') FOR [ModifiedBy]
GO

ALTER TABLE [dbo].[ProductVideo] ADD  CONSTRAINT [DF_ProductVideo_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

ALTER TABLE [dbo].[ProductVideo]  WITH CHECK ADD  CONSTRAINT [FK_ProductVideo_Video] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Video] ([Id])
GO

ALTER TABLE [dbo].[ProductVideo] CHECK CONSTRAINT [FK_ProductVideo_Video]
GO

ALTER TABLE [dbo].[ProductVideo]  WITH CHECK ADD  CONSTRAINT [FK_ProductVideo_tbl_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[tbl_Product] ([Id])
GO

ALTER TABLE [dbo].[ProductVideo] CHECK CONSTRAINT [FK_ProductVideo_tbl_Product]
GO

ALTER TABLE [dbo].[ProductVideo] ADD  CONSTRAINT [UK_ProductVideo] UNIQUE NONCLUSTERED
(
    [VideoId] ASC,
    [ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO