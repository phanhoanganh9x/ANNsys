USE [inventorymanagement]
GO

/****** Object:  Table [dbo].[Token]    Script Date: 7/20/2020 11:35:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Token](
    [UUID] [uniqueidentifier] NOT NULL,
    [SecretKey]  AS (CONVERT([nvarchar](6),floor(rand()*(1000000)))),
    [UserID] [int] NOT NULL,
    [Browser] [nvarchar](50) NULL,
    [Device] [nvarchar](50) NULL,
    [CreatedDate] [datetime] NOT NULL,
    [LastDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Token] PRIMARY KEY CLUSTERED 
(
    [UUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Token] ADD  CONSTRAINT [DF_Token_UUID]  DEFAULT (newid()) FOR [UUID]
GO

ALTER TABLE [dbo].[Token] ADD  CONSTRAINT [DF_Token_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Token] ADD  CONSTRAINT [DF_Token_LastDate]  DEFAULT (getdate()) FOR [LastDate]
GO

CREATE NONCLUSTERED INDEX [ID_LastTime] ON [Token]
(
    [LastDate] DESC
,   [CreatedDate] DESC
)
GO

CREATE NONCLUSTERED INDEX [ID_User] ON [Token]
(
    [UserID] ASC
)
GO