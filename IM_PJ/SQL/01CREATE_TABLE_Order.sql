CREATE NONCLUSTERED INDEX ID_Customer ON tbl_Order
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