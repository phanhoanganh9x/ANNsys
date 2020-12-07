USE [inventorymanagement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF (EXISTS (SELECT NULL AS DUMMY 
            FROM INFORMATION_SCHEMA.TABLES 
            WHERE TABLE_SCHEMA = 'dbo' 
            AND  TABLE_NAME = 'ViewOrder'))
BEGIN
    DROP TABLE [dbo].[ViewOrder]
END
GO

CREATE TABLE [dbo].[ViewOrder](
    [Phone] [nvarchar](15) NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
    [Status] [int] NOT NULL,
    [PreOrderId] [bigint] NULL,
    [OrderId] [int] NULL,
	[CreatedBy] [nvarchar](15) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](15) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
    [Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_ViewOrder] PRIMARY KEY CLUSTERED 
(
    [Phone]
,	[Id] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ViewOrder] ADD  CONSTRAINT [DF_ViewOrder_Status]  DEFAULT (0) FOR [Status]
GO

ALTER TABLE [dbo].[ViewOrder] ADD  CONSTRAINT [DF_ViewOrder_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[ViewOrder] ADD  CONSTRAINT [DF_ViewOrder_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

ALTER TABLE [dbo].[ViewOrder]  WITH CHECK ADD  CONSTRAINT [FK_ViewOrder_PreOrder] FOREIGN KEY([PreOrderId])
REFERENCES [dbo].[PreOrder] ([Id])
GO

ALTER TABLE [dbo].[ViewOrder] CHECK CONSTRAINT [FK_ViewOrder_PreOrder]
GO

ALTER TABLE [dbo].[ViewOrder]  WITH CHECK ADD  CONSTRAINT [FK_ViewOrder_tbl_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[tbl_Order] ([Id])
GO

ALTER TABLE [dbo].[ViewOrder] CHECK CONSTRAINT [FK_ViewOrder_tbl_Order]
GO
