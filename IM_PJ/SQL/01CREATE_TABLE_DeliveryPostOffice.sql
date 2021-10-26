SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliveryPostOffice](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[NumberID] [nvarchar](50) NOT NULL,
	[Customer] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[Town] [nvarchar](50) NOT NULL,
	[Ward] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
	[DeliveryStatus] [nvarchar](50) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[ExpectDate] [datetime] NOT NULL,
	[COD] [money] NOT NULL,
	[OrderCOD] [money] NOT NULL,
	[Fee] [money] NOT NULL,
	[OrderFee] [money] NOT NULL,
	[Staff] [nvarchar](100) NOT NULL,
	[Review] [int] NOT NULL,
	[OrderStatus] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DeliveryPostOffice] ADD  CONSTRAINT [PK_DeliveryPostOffice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DeliveryPostOffice] ADD  CONSTRAINT [DF_DeliveryPostOffice_StartDate]  DEFAULT (getdate()) FOR [StartDate]
GO
ALTER TABLE [dbo].[DeliveryPostOffice] ADD  CONSTRAINT [DF_DeliveryPostOffice_ExpectDate]  DEFAULT (getdate()) FOR [ExpectDate]
GO
ALTER TABLE [dbo].[DeliveryPostOffice] ADD  CONSTRAINT [DF_DeliveryPostOffice_COD]  DEFAULT ((0)) FOR [COD]
GO
ALTER TABLE [dbo].[DeliveryPostOffice] ADD  CONSTRAINT [DF_DeliveryPostOffice_OrderCOD]  DEFAULT ((0)) FOR [OrderCOD]
GO
ALTER TABLE [dbo].[DeliveryPostOffice] ADD  CONSTRAINT [DF_DeliveryPostOffice_Fee]  DEFAULT ((0)) FOR [Fee]
GO
ALTER TABLE [dbo].[DeliveryPostOffice] ADD  CONSTRAINT [DF_DeliveryPostOffice_OrderFee]  DEFAULT ((0)) FOR [OrderFee]
GO
ALTER TABLE [dbo].[DeliveryPostOffice] ADD  CONSTRAINT [DF_DeliveryPostOffice_Review]  DEFAULT ((1)) FOR [Review]
GO
ALTER TABLE [dbo].[DeliveryPostOffice] ADD  CONSTRAINT [DF_DeliveryPostOffice_OrderStatus]  DEFAULT ((0)) FOR [OrderStatus]
GO

-- 2021-10-19: Đối ứng trường hợp đơn hàng nhóm của shop ANN
ALTER TABLE [dbo].[DeliveryPostOffice]
ALTER COLUMN [OrderID] [int] NULL

ALTER TABLE [dbo].[DeliveryPostOffice]
ADD [GroupOrderCode] [nvarchar](6) NULL