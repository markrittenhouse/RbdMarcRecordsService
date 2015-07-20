CREATE TABLE [dbo].[WebR2LibraryMarcRecords](
	[dailyMarcRecordFileId] [int] IDENTITY(1,1) NOT NULL,
	[isbn] [varchar](20) NULL,
	[isbn10] [varchar](20) NULL,
	[isbn13] [varchar](20) NULL,
	[eIsbn] [varchar](20) NULL,
	[marcRecordProviderTypeId] [int] NULL,
	[fileData] [varchar](max) NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[updatedDate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

INSERT INTO MarcRecordProviderType ([marcRecordProviderTypeId],[description],[priority])
     VALUES(4,'R2Library',1000);
INSERT INTO MarcRecordProviderType ([marcRecordProviderTypeId],[description],[priority])
     VALUES(5,'R2Library OCLC',1001);


CREATE TABLE [dbo].[OclcR2LibraryMarcRecords](
	[dailyMarcRecordFileId] [int] IDENTITY(1,1) NOT NULL,
	[isbn] [varchar](20) NULL,
	[isbn10] [varchar](20) NULL,
	[isbn13] [varchar](20) NULL,
	[eIsbn] [varchar](20) NULL,
	[marcRecordProviderTypeId] [int] NULL,
	[fileData] [varchar](max) NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[updatedDate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
