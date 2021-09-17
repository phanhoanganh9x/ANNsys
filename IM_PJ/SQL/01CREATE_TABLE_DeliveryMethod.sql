USE [inventorymanagement]
GO

/****** Object:  Table [dbo].[DeliveryMethod]    Script Date: 9/13/2021 5:48:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeliveryMethod](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Code] [nvarchar](50) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_DeliveryMethod] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DeliveryMethod] ADD  CONSTRAINT [DF_DeliveryMethod_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[DeliveryMethod] ADD  CONSTRAINT [DF_DeliveryMethod_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

SET IDENTITY_INSERT [dbo].[DeliveryMethod] ON

INSERT INTO [dbo].[DeliveryMethod] ([Id], [Code], [Name]) VALUES (1, N'Face', N'Lấy trực tiếp')
INSERT INTO [dbo].[DeliveryMethod] ([Id], [Code], [Name]) VALUES (2, N'VNPost', N'Bưu điện Việt Nam')
INSERT INTO [dbo].[DeliveryMethod] ([Id], [Code], [Name]) VALUES (3, N'Proship', N'Dịch vụ Proship')
INSERT INTO [dbo].[DeliveryMethod] ([Id], [Code], [Name]) VALUES (4, N'TransferStation', N'Chuyển xe')
INSERT INTO [dbo].[DeliveryMethod] ([Id], [Code], [Name]) VALUES (5, N'Shipper', N'Nhân viên giao')
INSERT INTO [dbo].[DeliveryMethod] ([Id], [Code], [Name]) VALUES (6, N'GHTK', N'Giao hàng tiết kiệm')
INSERT INTO [dbo].[DeliveryMethod] ([Id], [Code], [Name]) VALUES (7, N'Viettel', N'Viettel')
INSERT INTO [dbo].[DeliveryMethod] ([Id], [Code], [Name]) VALUES (8, N'Grab', N'Grab')
INSERT INTO [dbo].[DeliveryMethod] ([Id], [Code], [Name]) VALUES (9, N'AhaMove', N'AhaMove')
INSERT INTO [dbo].[DeliveryMethod] ([Id], [Code], [Name]) VALUES (10, N'J&T', N'J&T Express')
INSERT INTO [dbo].[DeliveryMethod] ([Id], [Code], [Name]) VALUES (11, N'GHN', N'Giao hàng nhanh toàn quốc')

SET IDENTITY_INSERT [dbo].[DeliveryMethod] OFF