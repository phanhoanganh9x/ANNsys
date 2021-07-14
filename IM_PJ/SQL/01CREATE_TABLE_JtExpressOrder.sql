USE [inventorymanagement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF (EXISTS (SELECT NULL AS DUMMY
            FROM INFORMATION_SCHEMA.TABLES
            WHERE TABLE_SCHEMA = 'dbo'
            AND  TABLE_NAME = 'JtExpressOrder'))
BEGIN
    DROP TABLE [dbo].[JtExpressOrder]
END
GO

CREATE TABLE [dbo].[JtExpressOrder](
    [OrderId] [int] NOT NULL,
    [Code] [nvarchar](100) NOT NULL,
    [SenderName] [nvarchar](100) NOT NULL,
    [SenderPhone] [nvarchar](15) NOT NULL,
    [SenderAddress] [nvarchar](MAX) NOT NULL,
    [SenderProvince] [nvarchar](100) NOT NULL,
    [SenderDistrict] [nvarchar](100) NOT NULL,
    [SenderWard] [nvarchar](100) NULL,
    [ReceiverName] [nvarchar](100) NOT NULL,
    [ReceiverPhone] [nvarchar](15) NOT NULL,
    [ReceiverAddress] [nvarchar](MAX) NOT NULL,
    [ReceiverProvince] [nvarchar](100) NOT NULL,
    [ReceiverDistrict] [nvarchar](100) NOT NULL,
    [ReceiverWard] [nvarchar](100) NULL,
    [PostalCode] [nvarchar](100) NULL,
    [PostalBranchCode] [nvarchar](100) NULL,
    [Fee] [money] NOT NULL,
    [InsuranceFee] [money] NOT NULL,
    [Cod] [money] NOT NULL,
    [CodFee] [money] NOT NULL,
    [ItemName] [nvarchar](100) NULL,
    [ItemNumber] [int] NOT NULL,
    [Weight] [float] NOT NULL,
    [Note] [nvarchar](MAX) NULL,
    [CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_JtExpressOrder] PRIMARY KEY CLUSTERED
(
    [OrderId] ASC
,   [Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[JtExpressOrder]  WITH CHECK ADD  CONSTRAINT [FK_JtExpressOrder_tbl_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[tbl_Order] ([ID])
GO

ALTER TABLE [dbo].[JtExpressOrder] CHECK CONSTRAINT [FK_JtExpressOrder_tbl_Order]
GO

CREATE UNIQUE INDEX [UC_JtExpressOrder_Order]
ON [dbo].[JtExpressOrder]([OrderId]);
GO