USE [inventorymanagement]
GO

/****** Object:  Table [dbo].[ExchangeRate]    Script Date: 31/10/2020 11:35:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ExchangeRate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CurrencyName] [nvarchar](100) NOT NULL,
	[CurrencyCode] [nvarchar](10) NOT NULL,
	[SellingRate] [money] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_ExchangeRate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExchangeRate] ADD  CONSTRAINT [AK_ExchangeRate_CurrencyCode]  UNIQUE ([CurrencyCode])
GO

ALTER TABLE [dbo].[ExchangeRate] ADD  CONSTRAINT [DF_ExchangeRate_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[ExchangeRate] ADD  CONSTRAINT [DF_ExchangeRate_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
