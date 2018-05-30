


USE [MarcRecords]
GO

/****** Object:  Table [dbo].[MarcRecordProvider]    Script Date: 5/24/2018 10:04:56 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MarcRecordProviderUpdate](
	[marcRecordProviderId] [int] NOT NULL,
	[encodingLevel] [varchar](1) NULL,
	[dateUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK__MarcRecordProviderUpdate)marcRecordProviderId] PRIMARY KEY NONCLUSTERED 
(
	[marcRecordProviderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO




USE [MarcRecords]
GO

/****** Object:  Table [dbo].[MarcRecordFile]    Script Date: 5/24/2018 10:06:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MarcRecordFileUpdate](
	[marcRecordProviderId] [int] NOT NULL,
	[marcRecordFileTypeId] [int] NOT NULL,
	[fileData] [varchar](max) NOT NULL,
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

