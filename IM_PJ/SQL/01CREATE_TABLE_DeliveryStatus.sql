USE [inventorymanagement]
GO

/****** Object:  Table [dbo].[DeliveryStatus]    Script Date: 9/13/2021 5:48:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeliveryStatus](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_DeliveryStatus] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DeliveryStatus] ADD  CONSTRAINT [DF_DeliveryStatus_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[DeliveryStatus] ADD  CONSTRAINT [DF_DeliveryStatus_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

INSERT INTO [dbo].[DeliveryStatus] ([Name]) VALUES (N'Gửi đi')
INSERT INTO [dbo].[DeliveryStatus] ([Name]) VALUES (N'Chuyển hoàn')
