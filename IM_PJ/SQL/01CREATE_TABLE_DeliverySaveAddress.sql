ALTER TABLE [dbo].[DeliverySaveAddress]
ALTER COLUMN [Type] INT NULL

INSERT INTO [dbo].[DeliverySaveAddress] (ID, Name, PID, Type, Region, Alias, IsPicked, IsDelivered)
VALUES (-1, N'Khác', NULL, NULL, NULL, N'Khác', 1, 1)