
USE [MarcRecords]
GO

/****** Object:  Table [dbo].[MarcRecordDataField]    Script Date: 5/17/2018 1:49:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MarcRecordDataField](
	[marcRecordDataFieldId] [int] NOT NULL,
	[marcRecordId] [int] NOT NULL,
	[marcRecordProviderTypeId] [int] NOT NULL,
	[fieldNumber] [varchar](10) NOT NULL,
	[fieldIndicator] [varchar](10) NULL,
	[marcValue] [varchar](max) NOT NULL,
	[dateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_MarcRecordDataField] PRIMARY KEY CLUSTERED 
(
	[marcRecordDataFieldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MarcRecordDataSubField](
	[marcRecordDataSubFieldsId] [int] NOT NULL,
	[marcRecordDataFieldId] int not null,
	[subFieldIndicator] [varchar](10) NOT NULL,
	[subFieldValue] [varchar](max) NOT NULL,
 CONSTRAINT [PK_MarcRecordDataSubFields] PRIMARY KEY CLUSTERED 
(
	[marcRecordDataSubFieldsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[MarcRecordDataSubField]  WITH CHECK ADD  CONSTRAINT [FK_MarcRecordDataField_MarcRecordDataFieldId] FOREIGN KEY([marcRecordDataFieldId])
REFERENCES [dbo].[MarcRecordDataField] ([marcRecordDataFieldId])
GO

ALTER TABLE [dbo].[MarcRecordDataSubField] CHECK CONSTRAINT [FK_MarcRecordDataField_MarcRecordDataFieldId]
GO