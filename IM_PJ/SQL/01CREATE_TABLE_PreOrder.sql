USE [inventorymanagement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PreOrder](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
    [Avatar] [nvarchar](MAX) NULL,
    [Status] [int] NOT NULL,
    [MethodDelivery] [int] NOT NULL,
    [PaymentMethod] [int] NOT NULL,
    [DeliveryAddressId] [bigint] NULL,
    [TotalQuantity] [int] NOT NULL,
    [TotalCostOfGood] [money] NOT NULL,
    [TotalPrice] [money] NOT NULL,
    [TotalDiscount] [money] NOT NULL,
    [ShipFee] [money] NOT NULL,
    [Total] AS (ISNULL([TotalPrice], 0) - ISNULL([TotalDiscount], 0) + ISNULL([ShipFee], 0)),
	[IsReceived] [bit] NOT NULL,
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

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_Status]  DEFAULT (1) FOR [Status]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_TotalQuantity]  DEFAULT (0) FOR [TotalQuantity]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_TotalCostOfGood]  DEFAULT (0) FOR [TotalCostOfGood]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_TotalPrice]  DEFAULT (0) FOR [TotalPrice]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_TotalDiscount]  DEFAULT (0) FOR [TotalDiscount]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_ShipFee]  DEFAULT (0) FOR [ShipFee]
GO

ALTER TABLE [dbo].[PreOrder] ADD  CONSTRAINT [DF_PreOrder_IsReceived]  DEFAULT (0) FOR [IsReceived]
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
