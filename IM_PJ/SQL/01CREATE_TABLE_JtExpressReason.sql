SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[JtExpressReason](
    [Id] [nvarchar](10) NOT NULL,
    [Description] [nvarchar](255) NOT NULL,
    [SpecificOccurrence] [nvarchar](MAX) NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_JtExpressReason] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[JtExpressReason] ADD  CONSTRAINT [DF_JtExpressReason_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[JtExpressReason] ADD  CONSTRAINT [DF_JtExpressReason_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO

INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S01', N'JSON format not correct', N'logistics_interface ( Data not correct)');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S02', N'Key-parase not correct', N'data_digest not correct, please check again');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S03', N'Company ID not correct', N'eccompanyid Not correct');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S04', N'Illegal notification type', N'Service type msg_type in The value content is not in the scope specified by the document.');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S05', N'Illegal notification content', N'logistics_interface Medium content format is incorrect');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S06', N'Connection timed out', N'If the response is not received within 30 seconds when the order is created, it is considered to be timeout');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S07', N'The system is abnormal, please try again', N'Database exception or other system error');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S08', N'Illegale-commerce platform', N'Eccompanyid Error');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S09', N'No task data back', N'Return prompt information when querying data');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S10', N'Duplicate order number', N'Repeat order number prompt when placing an order');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S11', N'Duplicate waybill number', N'Repeat order number prompt when placing an order');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S12', N'Order status is GOT and cannot cancel order', N'Cancellation of an order cannot be cancelled when the order status is received');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'S15', N'API Update is missing "customerid"', N'Customerid not null');
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B99', N'Illegal logistics order number', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B01', N'Cannot operate, the current status is: waiting for confirmation', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B02', N'Cannot operate, the current status is: order', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B03', N'Cannot operate, the current status is: no order', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B04', N'Can not operate, the current state is: successful', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B05', N'Can not operate, the current state is: failure to collect', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B06', N'Cannot operate, the current status is: successful receipt', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B07', N'Cannot operate, the current status is: sign failure', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B08', N'Cannot operate, the current status is: the order has been canceled', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B09', N'Cannot operate, the waybill number is empty', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B10', N'Cannot operate, the signing information is empty (including the waybill number, signing name, signing time can not be empty)', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B001', N'eccompanyid Not null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B002', N'customerid Not null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B003', N'txlogisticid Not null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B004', N'ordertype Not null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B005', N'servicetype Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B006', N'Sender Name sender>name Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B007', N'Sender e-mail sender>mailbox Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B008', N'Send phone sender>mobile Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B009', N'Sender province sender>prov Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B010', N'Sender district sender>city Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B011', N'Sender area sender>area Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B012', N'Sender address sender>address Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B013', N'Receiver name receiver>name Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B014', N'Receiver Phone receiver>mobile Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B015', N'Receiver province receiver>prov Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B016', N'Receiver District receiver>city Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B017', N'Receiver Area receiver>area Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B018', N'Receiver Address receiver>address Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B019', N'Created order time createordertime Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B020', N'Send start time sendstarttime Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B021', N'Send end time sendendtime Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B022', N'Payment method paytype Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B023', N'Total quantity totalquantity Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B024', N'COD amount itemsvalue Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B025', N'Chinese Item names items>itemname Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B026', N'English Iteam name items>englishName Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B027', N'Item number items>number Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B028', N'Declared value（IDR）items>itemvalue Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B029', N'URL items>itemurl Not Null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B030', N'Sender area sender> area Not Exists', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B031', N'Receiver area sender>area Not Exists', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B031-STOP', N'Khu vực người nhận đang trong vùng dịch', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B035', N'items Errros', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B036', N'Receiver email receiver>postcode Errors', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B038', N'Customerid not null', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B041', N'Customerid don’t have set up part sign', NULL);
INSERT INTO [JtExpressReason]([Id], [Description], [SpecificOccurrence]) VALUES (N'B042', N'Customerid is wrong ', NULL);
