USE [inventorymanagement]
GO

/****** Object:  Table [dbo].[DeliveryManager]    Script Date: 9/13/2021 5:48:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeliveryManager](
    [OrderTypeId] [int] NOT NULL,
    [Code] [nvarchar](50) NOT NULL,
    [DeliveryMethodId] [int] NOT NULL,
    [ShippingCode] [nvarchar](100) NULL,
    [StatusId] [int] NOT NULL,
    [SentDate] [datetime] NULL,
    [RefundDate] [datetime] NULL,
    [CreatedDate] [datetime] NOT NULL,
    [CreatedBy] [nvarchar](50) NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DeliveryManager] PRIMARY KEY CLUSTERED 
(
    [OrderTypeId] ASC,
    [Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DeliveryManager] ADD  CONSTRAINT [DF_DeliveryManager_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[DeliveryManager] ADD  CONSTRAINT [DF_DeliveryManager_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

ALTER TABLE [dbo].[DeliveryManager]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryManager_OrderType] FOREIGN KEY([OrderTypeId])
REFERENCES [dbo].[OrderType] ([Id])
GO

ALTER TABLE [dbo].[DeliveryManager] CHECK CONSTRAINT [FK_DeliveryManager_OrderType]
GO

ALTER TABLE [dbo].[DeliveryManager]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryManager_DeliveryMethod] FOREIGN KEY([DeliveryMethodId])
REFERENCES [dbo].[DeliveryMethod] ([Id])
GO

ALTER TABLE [dbo].[DeliveryManager] CHECK CONSTRAINT [FK_DeliveryManager_DeliveryMethod]
GO

ALTER TABLE [dbo].[DeliveryManager]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryManager_DeliveryStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[DeliveryStatus] ([Id])
GO

ALTER TABLE [dbo].[DeliveryManager] CHECK CONSTRAINT [FK_DeliveryManager_DeliveryStatus]
GO
