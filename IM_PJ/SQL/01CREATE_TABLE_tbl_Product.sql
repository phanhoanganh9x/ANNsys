ALTER TABLE [dbo].[tbl_Product] ADD [ShortDescription] NVARCHAR(MAX) NULL

ALTER TABLE [dbo].[tbl_Product] ADD [ShopeeDescription] NVARCHAR(MAX) NULL

ALTER TABLE [dbo].[tbl_Product] ADD [FeaturedImage] NVARCHAR(MAX) NULL

-- 2021-10-19: Đối ứng tăng cường hiệu năng tìm kiếm
ALTER TABLE [dbo].[tbl_Product]
ALTER COLUMN [ProductSKU] NVARCHAR(50) NULL
GO

CREATE NONCLUSTERED INDEX [ID_tbl_Product_SKU] ON [dbo].[tbl_Product]
(
	[ProductSKU] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO