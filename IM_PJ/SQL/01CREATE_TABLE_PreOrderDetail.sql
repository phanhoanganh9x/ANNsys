USE [inventorymanagement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF (EXISTS (SELECT NULL AS DUMMY 
            FROM INFORMATION_SCHEMA.TABLES 
            WHERE TABLE_SCHEMA = 'dbo' 
            AND  TABLE_NAME = 'PreOrderDetail'))
BEGIN
    DROP TABLE [dbo].[PreOrderDetail]
END
GO

CREATE TABLE [dbo].[PreOrderDetail](
	[PreOrderId] [bigint] NOT NULL,
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [ProductId] [int] NOT NULL,
    [VariationId] [int] NULL,
    [SKU] [nvarchar](MAX) NOT NULL,
    [Name] [nvarchar](MAX) NULL,
    [Avatar] [nvarchar](MAX) NULL,
    [Color] [nvarchar](MAX) NULL,
    [Size] [nvarchar](MAX) NULL,
    [CostOfGoods] [money] NOT NULL,
    [Price] [money] NOT NULL,
    [OldPrice] [money] NULL,
    [Quantity] [int] NOT NULL,
    [TotalCostOfGoods] AS (ISNULL([Quantity], 0) * ISNULL([CostOfGoods], 0)),
    [TotalPrice] AS (ISNULL([Quantity], 0) * ISNULL([Price], 0)),
	[CreatedBy] [nvarchar](15) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](15) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
    [Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_PreOrderDetail] PRIMARY KEY CLUSTERED 
(
    [PreOrderId] DESC,
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PreOrderDetail] ADD  CONSTRAINT [DF_PreOrderDetail_CostOfGoods]  DEFAULT (0) FOR [CostOfGoods]
GO

ALTER TABLE [dbo].[PreOrderDetail] ADD  CONSTRAINT [DF_PreOrderDetail_Price]  DEFAULT (0) FOR [Price]
GO

ALTER TABLE [dbo].[PreOrderDetail] ADD  CONSTRAINT [DF_PreOrderDetail_Quantity]  DEFAULT (0) FOR [Quantity]
GO


ALTER TABLE [dbo].[PreOrderDetail] ADD  CONSTRAINT [DF_PreOrderDetail_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[PreOrderDetail] ADD  CONSTRAINT [DF_PreOrderDetail_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

ALTER TABLE [dbo].[PreOrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_PreOrderDetail_PreOrder] FOREIGN KEY([PreOrderId])
REFERENCES [dbo].[PreOrder] ([Id])
GO

ALTER TABLE [dbo].[PreOrderDetail] CHECK CONSTRAINT [FK_PreOrderDetail_PreOrder]
GO

ALTER TABLE [dbo].[PreOrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_PreOrderDetail_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[tbl_Product] ([ID])
GO

ALTER TABLE [dbo].[PreOrderDetail] CHECK CONSTRAINT [FK_PreOrderDetail_Product]
GO

ALTER TABLE [dbo].[PreOrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_PreOrderDetail_ProductVariation] FOREIGN KEY([VariationId])
REFERENCES [dbo].[tbl_ProductVariable] ([ID])
GO

ALTER TABLE [dbo].[PreOrderDetail] CHECK CONSTRAINT [FK_PreOrderDetail_ProductVariation]
GO

