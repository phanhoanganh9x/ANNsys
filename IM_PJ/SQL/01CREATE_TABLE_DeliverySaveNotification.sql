SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeliverySaveNotification](
    [GhtkCode] [nvarchar](100) NOT NULL,
    [OrderId] [int] NOT NULL,
    [StatusId] [int] NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
    [ReasonCode] [int] NULL,
    [Reason] [nvarchar](max) NULL,
    [Weight] [float] NOT NULL,
    [GhtkFee] [money] NOT NULL,
    [GhtkCod] [money] NULL,
    [ReturnPartPackage] [int] NOT NULL,
    [CustomerName] [nvarchar](255) NULL,
    [CustomerPhone] [nvarchar](20) NULL,
    [CustomerAddress] [nvarchar](max) NULL,
    [OrderDate] [datetime] NULL,
    [Cod] [money] NULL,
    [Fee] [money] NULL,
    [Staff] [nvarchar](15) NULL,
    [CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_DeliverySaveNotification] PRIMARY KEY CLUSTERED 
(
    [OrderId] ASC,
    [GhtkCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[DeliverySaveNotification] ADD  CONSTRAINT [DF_DeliverySaveNotification_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[DeliverySaveNotification] ADD  CONSTRAINT [DF_DeliverySaveNotification_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

ALTER TABLE [dbo].[DeliverySaveNotification]  WITH CHECK ADD  CONSTRAINT [FK_DeliverySaveNotification_tbl_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[tbl_Order] ([ID])
GO

ALTER TABLE [dbo].[DeliverySaveNotification] CHECK CONSTRAINT [FK_DeliverySaveNotification_tbl_Order]
GO

ALTER TABLE [dbo].[DeliverySaveNotification]  WITH CHECK ADD  CONSTRAINT [FK_DeliverySaveNotification_DeliverySaveStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[DeliverySaveStatus] ([Id])
GO

ALTER TABLE [dbo].[DeliverySaveNotification] CHECK CONSTRAINT [FK_DeliverySaveNotification_DeliverySaveStatus]
GO

ALTER TABLE [dbo].[DeliverySaveNotification]  WITH CHECK ADD  CONSTRAINT [FK_DeliverySaveNotification_DeliverySaveReason] FOREIGN KEY([ReasonCode])
REFERENCES [dbo].[DeliverySaveReason] ([Id])
GO

ALTER TABLE [dbo].[DeliverySaveNotification] CHECK CONSTRAINT [FK_DeliverySaveNotification_DeliverySaveReason]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Trạng thái của đơn GHTK' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliverySaveNotification', @level2type=N'COLUMN',@level2name=N'StatusId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngày nhân viên hoàn tất đơn hàng' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeliverySaveNotification', @level2type=N'COLUMN',@level2name=N'OrderDate'
GO

-- 2021-10-19: Đối ứng trường hợp đơn hàng nhóm của shop ANN
ALTER TABLE [dbo].[DeliverySaveNotification]
DROP CONSTRAINT [PK_DeliverySaveNotification]

ALTER TABLE [dbo].[DeliverySaveNotification]
ADD CONSTRAINT [PK_DeliverySaveNotification] PRIMARY KEY ([GhtkCode] ASC)

ALTER TABLE [dbo].[DeliverySaveNotification]
ALTER COLUMN [OrderId] [int] NULL

ALTER TABLE [dbo].[DeliverySaveNotification]
ADD [GroupOrderCode] [nvarchar](6) NULL

ALTER TABLE [dbo].[DeliverySaveNotification]  WITH CHECK ADD  CONSTRAINT [FK_DeliverySaveNotification_GroupOrder] FOREIGN KEY([GroupOrderCode])
REFERENCES [dbo].[GroupOrder] ([Code])
GO