USE [inventorymanagement]
GO

/****** Object:  Table [dbo].[OrderType]    Script Date: 9/13/2021 5:48:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderType](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_OrderType] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderType] ADD  CONSTRAINT [DF_OrderType_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[OrderType] ADD  CONSTRAINT [DF_OrderType_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

INSERT INTO [dbo].[OrderType] ([Name]) VALUES (N'ANN')
INSERT INTO [dbo].[OrderType] ([Name]) VALUES (N'Shopee')
INSERT INTO [dbo].[OrderType] ([Name]) VALUES (N'Lazada')
