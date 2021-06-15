SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeliverySaveStatus](
    [Id] [int] NOT NULL,
    [Name] [nvarchar](255) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_DeliverySaveStatus] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DeliverySaveStatus] ADD  CONSTRAINT [DF_DeliverySaveStatus_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[DeliverySaveStatus] ADD  CONSTRAINT [DF_DeliverySaveStatus_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES ( -1, N'Hủy đơn hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (  0, N'Trạng thái khác');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (  1, N'Chưa tiếp nhận');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (  2, N'Đã tiếp nhận');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (  3, N'Đã lấy hàng/Đã nhập kho');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (  4, N'Đã điều phối giao hàng/Đang giao hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (  5, N'Đã giao hàng/Chưa đối soát');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (  6, N'Đã đối soát');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (  7, N'Không lấy được hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (  8, N'Hoãn lấy hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (  9, N'Không giao được hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES ( 10, N'Delay giao hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES ( 11, N'Đã đối soát công nợ trả hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES ( 12, N'Đã điều phối lấy hàng/Đang lấy hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES ( 13, N'Đơn hàng bồi hoàn');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES ( 20, N'Đang trả hàng (COD cầm hàng đi trả)');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES ( 21, N'Đã trả hàng (COD đã trả xong hàng)');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (123, N'Shipper báo đã lấy hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (127, N'Shipper (nhân viên lấy/giao hàng) báo không lấy được hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (128, N'Shipper báo delay lấy hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES ( 45, N'Shipper báo đã giao hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES ( 49, N'Shipper báo không giao được giao hàng');
INSERT INTO [dbo].[DeliverySaveStatus] ([Id], [Name]) VALUES (410, N'Shipper báo delay giao hàng');