SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliverySaveReport](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MaDonHang] [nvarchar](100) NOT NULL,
	[MaDonHangShop] [int],
	[TrangThaiDonHang] [int] NOT NULL,
	[NgayTao] [datetime] NOT NULL,
	[NgayHoanThanh] [datetime] NOT NULL,
	[KhoiLuong] [decimal] NOT NULL,
	[KhachHang] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[DiaChi] [nvarchar](MAX) NOT NULL,
	[TienCod] [money] NOT NULL,
	[PhiBaoHiem] [money] NOT NULL,
	[PhiGiaoHang] [money] NOT NULL,
	[PhiDichVuTraTruoc] [money] NOT NULL,
	[PhiDichVuCanTru] [money] NOT NULL,
	[PhiDichVuHoanLai] [money] NOT NULL,
	[PhiChuyenHoan] [money] NOT NULL,
	[PhiThayDoiDiaChiGiaoHang] [money] NOT NULL,
	[PhiLuuKho] [money] NOT NULL,
	[TienTip] [money] NOT NULL,
	[PhiDonHoanDaThanhToan] [money] NOT NULL,
	[ShopTraShipKhiTraHang] [money] NOT NULL,
	[KhuyenMai] [money] NOT NULL,
	[ThanhToan] [money] NOT NULL,
	[GhiChu] [nvarchar](MAX),
	[FileName] [nvarchar](255) NOT NULL,
	[OrderSearchStatus] [int] NOT NULL,
	[ShopCod][money],
	[ShopFee] [money],
	[Staff] [nvarchar](100),
	[Review] [int] NOT NULL,
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DeliverySaveReport] ADD  CONSTRAINT [PK_DeliverySaveReport] PRIMARY KEY CLUSTERED
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DeliverySaveReport] ADD  CONSTRAINT [DF_DeliverySaveReport_Review]  DEFAULT ((1)) FOR [Review]
GO

EXEC sys.sp_addextendedproperty @name=N'TrangThaiDonHang', @value=N'1: Đã đối soát; 2: Đã đối soát công nợ trả hàng' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliverySaveReport', @level2type=N'COLUMN',@level2name=N'TrangThaiDonHang'
GO

EXEC sys.sp_addextendedproperty @name=N'OrderSearchStatus', @value=N'1: Tìm thấy đơn; 2: Không tìm thấy đơn' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliverySaveReport', @level2type=N'COLUMN',@level2name=N'OrderSearchStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'ShopCod', @value=N'Tiền COD của hệ thống ANN Shop' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliverySaveReport', @level2type=N'COLUMN',@level2name=N'ShopCod'
GO

EXEC sys.sp_addextendedproperty @name=N'ShopFee', @value=N'Tiền phí giao hàng mà hệ thống ANN Shop ghi nhận' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliverySaveReport', @level2type=N'COLUMN',@level2name=N'ShopFee'
GO

EXEC sys.sp_addextendedproperty @name=N'Review', @value=N'1: Chư duyệt; 2: Đã duyệt' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliverySaveReport', @level2type=N'COLUMN',@level2name=N'Review'
GO

CREATE UNIQUE INDEX UC_DeliverySaveReport_MaDonHang
ON [dbo].[DeliverySaveReport](MaDonHang);
GO

CREATE NONCLUSTERED INDEX [ID_DeliverySaveReport_FileName] ON [dbo].[DeliverySaveReport]
(
	[FileName] ASC,
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- 2021-10-19: Đối ứng trường hợp đơn hàng nhóm của shop ANN
ALTER TABLE [dbo].[DeliverySaveReport]
ADD [MaDonHangGopShop] [nvarchar](6) NULL