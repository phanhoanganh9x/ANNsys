USE [inventorymanagement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeliveryAddress](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](15) NOT NULL,
	[Address] [nvarchar](MAX) NULL,
	[ProvinceId] [int] NOT NULL,
	[DistrictId] [int] NOT NULL,
	[WardId] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[CreatedBy] [nvarchar](15) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](15) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL
 CONSTRAINT [PK_DeliveryAddress] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DeliveryAddress] ADD  CONSTRAINT [DF_DeliveryAddress_IsDefault]  DEFAULT (0) FOR [IsDefault]
GO

ALTER TABLE [dbo].[DeliveryAddress] ADD  CONSTRAINT [DF_DeliveryAddress_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[DeliveryAddress] ADD  CONSTRAINT [DF_DeliveryAddress_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

ALTER TABLE [dbo].[DeliveryAddress]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryAddress_Province] FOREIGN KEY([ProvinceId])
REFERENCES [dbo].[DeliverySaveAddress] ([ID])
GO

ALTER TABLE [dbo].[DeliveryAddress] CHECK CONSTRAINT [FK_DeliveryAddress_Province]
GO

ALTER TABLE [dbo].[DeliveryAddress]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryAddress_District] FOREIGN KEY([DistrictId])
REFERENCES [dbo].[DeliverySaveAddress] ([ID])
GO

ALTER TABLE [dbo].[DeliveryAddress] CHECK CONSTRAINT [FK_DeliveryAddress_District]
GO

ALTER TABLE [dbo].[DeliveryAddress]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryAddress_Ward] FOREIGN KEY([WardId])
REFERENCES [dbo].[DeliverySaveAddress] ([ID])
GO

ALTER TABLE [dbo].[DeliveryAddress] CHECK CONSTRAINT [FK_DeliveryAddress_Ward]
GO

