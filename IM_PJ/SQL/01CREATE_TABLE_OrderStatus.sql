USE [inventorymanagement]
GO

/****** Object:  Table [dbo].[OrderStatus]    Script Date: 9/13/2021 5:48:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderStatus](
    [Id] [int] IDENTITY(0,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [Index] [int] NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_OrderStatus] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderStatus] ADD  CONSTRAINT [DF_OrderStatus_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[OrderStatus] ADD  CONSTRAINT [DF_OrderStatus_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

SET IDENTITY_INSERT [dbo].[DeliveryMethod] ON

INSERT INTO [dbo].[OrderStatus] ([Name], [Index]) VALUES (0, N'Chờ tiếp nhận', 1)
INSERT INTO [dbo].[OrderStatus] ([Name], [Index]) VALUES (1, N'Đang xử lý', 2)
INSERT INTO [dbo].[OrderStatus] ([Name], [Index]) VALUES (2, N'Đã hoàn tất', 3)
INSERT INTO [dbo].[OrderStatus] ([Name], [Index]) VALUES (3, N'Đã hủy', 5)
INSERT INTO [dbo].[OrderStatus] ([Name], [Index]) VALUES (4, N'Chuyển hoàn', 6)
INSERT INTO [dbo].[OrderStatus] ([Name], [Index]) VALUES (5, N'Đã gửi hàng', 4)

SET IDENTITY_INSERT [dbo].[DeliveryMethod] OFF