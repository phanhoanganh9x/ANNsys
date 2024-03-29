﻿CREATE NONCLUSTERED INDEX ID_Customer ON tbl_Order
(
    [AgentID] ASC
,   [CustomerID] ASC
,   [CreatedDate] ASC
)
INCLUDE
(
    [ID]
)
;

ALTER TABLE tbl_Order
ADD [GhtkInsuranceFee] MONEY NULL;

UPDATE tbl_Order
SET [GhtkInsuranceFee] = 0


ALTER TABLE tbl_Order
ALTER COLUMN [GhtkInsuranceFee] MONEY NOT NULL;

ALTER TABLE [dbo].[tbl_Order] ADD  CONSTRAINT [DF_tbl_Order_GhtkInsuranceFee]  DEFAULT (0) FOR [GhtkInsuranceFee]
GO

-- Remove column GhtkInsuranceFee vì không cần lấy phí bảo hiểm nữa
ALTER TABLE [dbo].[tbl_Order] DROP  CONSTRAINT [DF_tbl_Order_GhtkInsuranceFee]
GO

ALTER TABLE [dbo].[tbl_Order] DROP COLUMN [GhtkInsuranceFee]
GO


-- 2021-10-14: Thêm cột mã code gộp đơn
ALTER TABLE [dbo].[tbl_Order] ADD [GroupCode] NVARCHAR(6) NULL;

ALTER TABLE [dbo].[tbl_Order]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Order_GroupOrder] FOREIGN KEY([GroupCode])
REFERENCES [dbo].[GroupOrder] ([Code])
GO

ALTER TABLE [dbo].[tbl_Order] CHECK CONSTRAINT [FK_tbl_Order_GroupOrder]
GO