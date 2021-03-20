ALTER TABLE [dbo].[tbl_Config] ADD [StaffIndex] [INT] NULL
GO

UPDATE [dbo].[tbl_Config]
SET [StaffIndex] = 0
GO

ALTER TABLE [dbo].[tbl_Config] ALTER COLUMN [StaffIndex] [INT] NOT NULL
GO

ALTER TABLE [dbo].[tbl_Config] ADD  CONSTRAINT [DF_tbl_Config_StaffIndex]  DEFAULT (0) FOR [StaffIndex]
GO