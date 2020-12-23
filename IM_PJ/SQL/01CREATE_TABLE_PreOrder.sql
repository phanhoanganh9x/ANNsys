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

IF (EXISTS (SELECT NULL AS DUMMY 
            FROM INFORMATION_SCHEMA.TABLES 
            WHERE TABLE_SCHEMA = 'dbo' 
            AND  TABLE_NAME = 'PreOrder'))
BEGIN
    DROP TABLE [dbo].[PreOrder]
END
GO

CREATE TABLE [dbo].[PreOrder](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [Status] [int] NOT NULL,
    [Avatar] [nvarchar](MAX) NULL,
    [DeliveryAddressId] [bigint] NOT NULL,
    [DeliveryMethod] [int] NOT NULL,
    [PaymentMethod] [int] NOT NULL,
    [CouponId] [int] NULL,
    [TotalQuantity] [int] NOT NULL,
    [TotalCostOfGoods] [money] NOT NULL,
    [TotalPrice] [money] NOT NULL,
    [TotalDiscount] [money] NOT NULL,
    [CouponPrice] [money] NOT NULL,
    [Total] AS (ISNULL([TotalPrice], 0) - ISNULL([TotalDiscount], 0) - ISNULL([CouponPrice], 0)),
    [SourceOrdering] [nvarchar](100) NOT NULL,
    [CreatedBy] [nvarchar](15) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](15) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    [Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_PreOrder] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_TotalQuantity]  DEFAULT (0) FOR [TotalQuantity]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_TotalCostOfGoods]  DEFAULT (0) FOR [TotalCostOfGoods]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_TotalPrice]  DEFAULT (0) FOR [TotalPrice]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_TotalDiscount]  DEFAULT (0) FOR [TotalDiscount]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_CouponPrice]  DEFAULT (0) FOR [CouponPrice]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

ALTER TABLE [dbo].[PreOrder]  WITH CHECK ADD  CONSTRAINT [FK_PreOrder_DeliveryAddress] FOREIGN KEY([DeliveryAddressId])
REFERENCES [dbo].[DeliveryAddress] ([Id])
GO

ALTER TABLE [dbo].[PreOrder] CHECK CONSTRAINT [FK_PreOrder_DeliveryAddress]
GO

ALTER TABLE [dbo].[PreOrder]  WITH CHECK ADD  CONSTRAINT [FK_PreOrder_Coupon] FOREIGN KEY([CouponId])
REFERENCES [dbo].[Coupon] ([Id])
GO

ALTER TABLE [dbo].[PreOrder] CHECK CONSTRAINT [FK_PreOrder_Coupon]
GO
