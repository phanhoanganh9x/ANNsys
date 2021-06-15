SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeliverySaveReason](
    [Id] [int] NOT NULL,
    [Name] [nvarchar](255) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_DeliverySaveReason] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DeliverySaveReason] ADD  CONSTRAINT [DF_DeliverySaveReason_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[DeliverySaveReason] ADD  CONSTRAINT [DF_DeliverySaveReason_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 100, N'Nhà cung cấp (NCC) hẹn lấy vào ca tiếp theo');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 101, N'GHTK không liên lạc được với NCC');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 102, N'NCC chưa có hàng');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 103, N'NCC đổi địa chỉ');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 104, N'NCC hẹn ngày lấy hàng');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 105, N'GHTK quá tải, không lấy kịp');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 106, N'Do điều kiện thời tiết, khách quan');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 107, N'Lý do khác');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 110, N'Địa chỉ ngoài vùng phục vụ');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 111, N'Hàng không nhận vận chuyển');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 112, N'NCC báo hủy');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 113, N'NCC hoãn/không liên lạc được 3 lần');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 114, N'Lý do khác');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 115, N'Đối tác hủy đơn qua API');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 120, N'GHTK quá tải, giao không kịp');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 121, N'Người nhận hàng hẹn giao ca tiếp theo');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 122, N'Không gọi được cho người nhận hàng');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 123, N'Người nhận hàng hẹn ngày giao');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 124, N'Người nhận hàng chuyển địa chỉ nhận mới');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 125, N'Địa chỉ người nhận sai, cần NCC check lại');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 126, N'Do điều kiện thời tiết, khách quan');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 127, N'Lý do khác');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 128, N'Đối tác hẹn thời gian giao hàng');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 129, N'Không tìm thấy hàng');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES (1200, N'SĐT người nhận sai, cần NCC check lại');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES (  30, N'Người nhận không đồng ý nhận sản phẩm');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 131, N'Không liên lạc được với KH 3 lần');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 132, N'KH hẹn giao lại quá 3 lần');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 133, N'Shop báo hủy đơn hàng');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 134, N'Lý do khác');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 135, N'Đối tác hủy đơn qua API');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 140, N'NCC hẹn trả ca sau');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 141, N'Không liên lạc được với NCC');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 142, N'NCC không có nhà');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 143, N'NCC hẹn ngày trả');
INSERT INTO [dbo].[DeliverySaveReason] ([Id], [Name]) VALUES ( 144, N'Lý do khác');