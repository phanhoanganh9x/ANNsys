USE [inventorymanagement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF (EXISTS (SELECT NULL AS DUMMY
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_SCHEMA = 'dbo'
            AND  TABLE_NAME = 'GroupOrder'))
BEGIN
    DROP TABLE [dbo].[GroupOrder]
END
GO

CREATE TABLE [dbo].[GroupOrder](
    [Code] [nvarchar](6) NOT NULL,
    [CustomerId] [int] NOT NULL,
    [DeliveryAddressId] [bigint] NOT NULL,
    [OrderStatusId] [int] NOT NULL,
    [PaymentStatusId] [int] NOT NULL,
    [PaymentMethodId] [int] NOT NULL,
    [DeliveryMethodId] [int] NOT NULL,
    [ShippingCode] [nvarchar](100) NULL,
    [ShippingFee] [money] NOT NULL,
    [CreatedBy] [nvarchar](15) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](15) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL
 CONSTRAINT [PK_GroupOrder] PRIMARY KEY CLUSTERED
(
    [Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GroupOrder] ADD  CONSTRAINT [DF_GroupOrder_ShippingFee]  DEFAULT (0) FOR [ShippingFee]
GO

ALTER TABLE [dbo].[GroupOrder] ADD  CONSTRAINT [DF_GroupOrder_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[GroupOrder] ADD  CONSTRAINT [DF_GroupOrder_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

ALTER TABLE [dbo].[GroupOrder]  WITH CHECK ADD  CONSTRAINT [FK_GroupOrder_tbl_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[tbl_Customer] ([ID])
GO

ALTER TABLE [dbo].[GroupOrder] CHECK CONSTRAINT [FK_GroupOrder_tbl_Customer]
GO

ALTER TABLE [dbo].[GroupOrder]  WITH CHECK ADD  CONSTRAINT [FK_GroupOrder_DeliveryAddress] FOREIGN KEY([DeliveryAddressId])
REFERENCES [dbo].[DeliveryAddress] ([ID])
GO

ALTER TABLE [dbo].[GroupOrder] CHECK CONSTRAINT [FK_GroupOrder_DeliveryAddress]
GO

ALTER TABLE [dbo].[GroupOrder]  WITH CHECK ADD  CONSTRAINT [FK_GroupOrder_OrderStatus] FOREIGN KEY([OrderStatusId])
REFERENCES [dbo].[OrderStatus] ([ID])
GO

ALTER TABLE [dbo].[GroupOrder] CHECK CONSTRAINT [FK_GroupOrder_OrderStatus]
GO

ALTER TABLE [dbo].[GroupOrder]  WITH CHECK ADD  CONSTRAINT [FK_GroupOrder_DeliveryMethod] FOREIGN KEY([DeliveryMethodId])
REFERENCES [dbo].[DeliveryMethod] ([ID])
GO

ALTER TABLE [dbo].[GroupOrder] CHECK CONSTRAINT [FK_GroupOrder_DeliveryMethod]
GO

-- 2021-10-19: Đối ứng trường hợp đơn hàng nhóm của shop ANN
ALTER TABLE [dbo].[GroupOrder]
ADD [Weight] [float] NULL

ALTER TABLE [dbo].[GroupOrder]
ADD [Cod] [money] NULL

ALTER TABLE [dbo].[GroupOrder] ADD  CONSTRAINT [DF_GroupOrder_Cod]  DEFAULT (0) FOR [Cod]
GO

UPDATE [dbo].[GroupOrder]
SET [Weight] = 0
,   [Cod] = 0

ALTER TABLE [dbo].[GroupOrder]
ALTER COLUMN [Weight] [float] NOT NULL

ALTER TABLE [dbo].[GroupOrder]
ALTER COLUMN [Cod] [money] NOT NULL

ALTER TABLE [dbo].[GroupOrder]
ALTER COLUMN [Weight] [float] NOT NULL

ALTER TABLE [dbo].[GroupOrder]
ADD [TransportId] [int] NULL

ALTER TABLE [dbo].[GroupOrder]
ADD [TransportBranchId] [int] NULL

ALTER TABLE [dbo].[GroupOrder]  WITH CHECK ADD  CONSTRAINT [FK_GroupOrder_Transport] FOREIGN KEY([TransportId], [TransportBranchId])
REFERENCES [dbo].[tbl_TransportCompany] ([ID], [SubID])
GO

ALTER TABLE [dbo].[GroupOrder] CHECK CONSTRAINT [FK_GroupOrder_Transport]
GO

ALTER TABLE [dbo].[GroupOrder]
ADD [PostalDeliveryMethodId] [int] NULL
