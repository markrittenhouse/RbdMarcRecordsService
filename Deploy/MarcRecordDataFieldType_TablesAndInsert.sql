
CREATE TABLE MarcRecordDataFieldType(
	[marcRecordDataFieldTypeId] [int] IDENTITY(1,1) NOT NULL,
	[fieldNumber] [varchar](10) not null,
	[description] [varchar](255) NOT NULL,
	[indicator1] [varchar](255) NOT NULL,
	[indicator2] [varchar](255) NOT NULL,
 CONSTRAINT [PK_MarcRecordDataFieldType] PRIMARY KEY CLUSTERED 
(
	[marcRecordDataFieldTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]



CREATE TABLE MarcRecordDataSubFieldType(
	[marcRecordDataSubFieldTypeId] [int] IDENTITY(1,1) NOT NULL,
	[fieldNumber] [varchar](10) not null,
	[indicator] [varchar](255) NOT NULL,
	[description] [varchar](255) NOT NULL
 CONSTRAINT [PK_MarcRecordDataSubFieldType] PRIMARY KEY CLUSTERED 
(
	[marcRecordDataSubFieldTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

go 

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('001', 'CONTROL NUMBER', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('002', 'LOCALLY DEFINED (UNOFFICIAL)', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('003', 'CONTROL NUMBER IDENTIFIER', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('005', 'DATE AND TIME OF LATEST TRANSACTION', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('006', 'FIXED-LENGTH DATA ELEMENTS--ADDITIONAL MATERIAL CHARACTERISTICS--GENERAL INFORMATION', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('007', 'PHYSICAL DESCRIPTION FIXED FIELD--GENERAL INFORMATION', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('008', 'FIXED-LENGTH DATA ELEMENTS--GENERAL INFORMATION', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('010', 'LIBRARY OF CONGRESS CONTROL NUMBER', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('010', '$a', 'LC control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('010', '$b', 'NUCMC control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('010', '$z', 'Canceled/invalid LC control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('010', '$8', 'Field link and sequence number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('013', 'PATENT CONTROL INFORMATION', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('013', '$a', 'Number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('013', '$b', 'Country');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('013', '$c', 'Type of number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('013', '$d', 'Date');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('013', '$e', 'Status');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('013', '$f', 'Party to document');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('013', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('013', '$8', 'Field link and sequence number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('015', 'NATIONAL BIBLIOGRAPHY NUMBER', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('015', '$a', 'National bibliography number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('015', '$z', 'Canceled/Invalid national bibliographic number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('015', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('015', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('015', '$8', 'Field link and sequence number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('016', 'NATIONAL BIBLIOGRAPHIC AGENCY CONTROL NUMBER', 'National bibliographic agency', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('016', '$a', 'Record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('016', '$z', 'Canceled or invalid record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('016', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('016', '$8', 'Field link and sequence number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('017', 'COPYRIGHT OR LEGAL DEPOSIT NUMBER', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('017', '$a', 'Copyright registration number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('017', '$b', 'Assigning agency');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('017', '$d', 'Date');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('017', '$i', 'Display Text');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('017', '$z', 'Canceled/invalid copyright or legal deposit number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('017', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('017', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('017', '$8', 'Field link and sequence number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('018', 'COPYRIGHT ARTICLE-FEE CODE', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('018', '$a', 'Copyright article-fee code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('018', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('018', '$8', 'Field link and sequence number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('020', 'INTERNATIONAL STANDARD BOOK NUMBER', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('020', '$a', 'International Standard Book Number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('020', '$c', 'Terms of availability');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('020', '$z', 'Canceled/invalid ISBN');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('020', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('020', '$8', 'Field link and sequence number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('022', 'INTERNATIONAL STANDARD SERIAL NUMBER', 'Level of international interest', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('022', '$a', 'International Standard Serial Number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('022', '$l', 'ISSN-L');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('022', '$m', 'Canceled ISSN-L');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('022', '$y', 'Incorrect ISSN');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('022', '$z', 'Canceled ISSN');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('022', '$2', 'Source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('022', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('022', '$8', 'Field link and sequence number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('024', 'OTHER STANDARD IDENTIFIER', 'Type of standard number or code', 'Difference indicator');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('024', '$a', 'Standard number or code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('024', '$c', 'Terms of availability');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('024', '$d', 'Additional codes following the standard number or code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('024', '$z', 'Canceled/invalid standard number or code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('024', '$2', 'Source of number or code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('024', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('024', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('025', 'OVERSEAS ACQUISITION NUMBER', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('025', '$a', 'Overseas acquisition number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('025', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('026', 'FINGERPRINT IDENTIFIER', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('026', '$a', 'First and second groups of characters');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('026', '$b', 'Third and fourth groups of characters');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('026', '$c', 'Date');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('026', '$d', 'Number of volume or part');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('026', '$e', 'Unparsed fingerprint');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('026', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('026', '$5', 'Institution to which field applies');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('026', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('026', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('027', 'STANDARD TECHNICAL REPORT NUMBER', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('027', '$a', 'Standard technical report number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('027', '$z', 'Canceled/invalid number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('027', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('027', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('028', 'PUBLISHER NUMBER', 'Type of publisher number', 'Note/added entry controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('028', '$a', 'Publisher number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('028', '$b', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('028', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('028', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('030', 'CODEN DESIGNATION', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('030', '$a', 'CODEN');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('030', '$z', 'Canceled/invalid CODEN');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('030', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('030', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('031', 'MUSICAL INCIPITS INFORMATION', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$a', 'Number of work');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$b', 'Number of movement');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$c', 'Number of excerpt');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$d', 'Caption or heading');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$e', 'Role');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$g', 'Clef');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$m', 'Voice/instrument');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$n', 'Key signature');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$o', 'Time signature');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$p', 'Musical notation');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$q', 'General note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$r', 'Key or mode');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$s', 'Coded validity note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$t', 'Text incipit');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$u', 'Uniform Resource Identifier');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$y', 'Link text');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$z', 'Public note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$2', 'System code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('031', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('032', 'POSTAL REGISTRATION NUMBER', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('032', '$a', 'Postal registration number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('032', '$b', 'Source (agency assigning number)');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('032', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('032', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('033', 'DATE/TIME AND PLACE OF AN EVENT', 'Type of date in subfield $a', 'Type of event');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('033', '$a', 'Formatted date/time');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('033', '$b', 'Geographic classification area code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('033', '$c', 'Geographic classification subarea code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('033', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('033', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('033', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('034', 'CODED CARTOGRAPHIC MATHEMATICAL DATA', 'Type of scale', 'Type of ring');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$a', 'Category of scale');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$b', 'Constant ratio linear horizontal scale');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$c', 'Constant ratio linear vertical scale');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$d', 'Coordinates--westernmost longitude');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$e', 'Coordinates--easternmost longitude');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$f', 'Coordinates--northernmost latitude');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$g', 'Coordinates--southernmost latitude');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$h', 'Angular scale');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$j', 'Declination--northern limit');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$k', 'Declination--southern limit');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$m', 'Right ascension--eastern limit');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$n', 'Right ascension--western limit');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$p', 'Equinox');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$s', 'G-ring latitude');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$t', 'G-ring longitude');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$x', 'Beginning date');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$y', 'Ending date');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$z', 'Name of extraterrestial body');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('034', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('035', 'SYSTEM CONTROL NUMBER', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('035', '$a', 'System control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('035', '$z', 'Canceled/invalid control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('035', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('035', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('036', 'ORIGINAL STUDY NUMBER FOR COMPUTER DATA FILES', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('036', '$a', 'Original study number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('036', '$b', 'Source (agency assigning number) ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('036', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('036', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('037', 'SOURCE OF ACQUISITION', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('037', '$a', 'Stock number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('037', '$b', 'Source of stock number/acquisition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('037', '$c', 'Terms of availability ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('037', '$f', 'Form of issue ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('037', '$g', 'Additional format characteristics ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('037', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('037', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('037', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('038', 'RECORD CONTENT LICENSOR', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('038', '$a', 'Record content licensor ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('038', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('038', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('040', 'CATALOGING SOURCE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('040', '$a', 'Original cataloging agency ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('040', '$b', 'Language of cataloging ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('040', '$c', 'Transcribing agency ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('040', '$d', 'Modifying agency ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('040', '$e', 'Description conventions ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('040', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('040', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('041', 'LANGUAGE CODE', 'Translation indication', 'Source of code');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('041', '$a', 'Language code of text/sound track or separate title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('041', '$b', 'Language code of summary or abstract');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('041', '$d', 'Language code of sung or spoken text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('041', '$e', 'Language code of librettos ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('041', '$f', 'Language code of table of contents ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('041', '$g', 'Language code of accompanying material other than librettos ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('041', '$h', 'Language code of original and/or intermediate translations of text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('041', '$j', 'Language code of subtitles or captions');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('041', '$2', 'Source of code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('041', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('041', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('042', 'AUTHENTICATION CODE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('042', '$a', 'Authentication code ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('043', 'GEOGRAPHIC AREA CODE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('043', '$a', 'Geographic area code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('043', '$b', 'Local GAC code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('043', '$c', 'ISO code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('043', '$2', 'Source of local code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('043', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('043', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('044', 'COUNTRY OF PUBLISHING/PRODUCING ENTITY CODE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('044', '$a', 'MARC country code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('044', '$b', 'Local subentity code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('044', '$c', 'ISO country code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('044', '$2', 'Source of local subentity code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('044', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('044', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('045', 'TIME PERIOD OF CONTENT', 'Type of time period in subfield $b or $c', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('045', '$a', 'Time period code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('045', '$b', 'Formatted 9999 B.C. through C.E. time period ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('045', '$c', 'Formatted pre-9999 B.C. time period ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('045', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('045', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('046', 'SPECIAL CODED DATES', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$a', 'Type of date code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$b', 'Date 1 (B.C. date) ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$c', 'Date 1 (C.E. date) ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$d', 'Date 2 (B.C. date) ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$e', 'Date 2 (C.E. date) ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$j', 'Date resource modified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$k', 'Beginning or single date created ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$l', 'Ending date created ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$m', 'Beginning of date valid ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$n', 'End of date valid ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$2', 'Source of date ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('046', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('047', 'FORM OF MUSICAL COMPOSITION CODE', 'Undefined', 'Source of code');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('047', '$a', 'Form of musical composition code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('047', '$2', 'Source of code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('047', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('048', 'NUMBER OF MUSICAL INSTRUMENTS OR VOICES CODE', 'Undefined', 'Source specified in subfield $2');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('048', '$a', 'Performer or ensemble ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('048', '$b', 'Soloist ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('048', '$2', 'Source of code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('048', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('050', 'LIBRARY OF CONGRESS CALL NUMBER', 'Existence in LC collection', 'Source of call number');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('050', '$a', 'Classification number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('050', '$b', 'Item number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('050', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('050', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('050', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('051', 'LIBRARY OF CONGRESS COPY, ISSUE, OFFPRINT STATEMENT', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('051', '$a', 'Classification number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('051', '$b', 'Item number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('051', '$c', 'Copy information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('051', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('052', 'GEOGRAPHIC CLASSIFICATION', 'Code source', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('052', '$a', 'Geographic classification area code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('052', '$b', 'Geographic classification subarea code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('052', '$d', 'Populated place name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('052', '$2', 'Code source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('052', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('052', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('055', 'CLASSIFICATION NUMBERS ASSIGNED IN CANADA', 'Existence in LAC collection', 'Type, completeness, source of class/call number');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('055', '$a', 'Classification number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('055', '$b', 'Item number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('055', '$2', 'Source of call/class number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('055', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('060', 'NATIONAL LIBRARY OF MEDICINE CALL NUMBER', 'Existence in NLM collection', 'Source of call number');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('060', '$a', 'Classification number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('060', '$b', 'Item number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('060', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('061', 'NATIONAL LIBRARY OF MEDICINE COPY STATEMENT', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('061', '$a', 'Classification number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('061', '$b', 'Item number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('061', '$c', 'Copy information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('061', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('066', 'CHARACTER SETS PRESENT', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('066', '$a', 'Primary G0 character set ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('066', '$b', 'Primary G1 character set ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('066', '$c', 'Alternate G0 or G1 character set ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('070', 'NATIONAL AGRICULTURAL LIBRARY CALL NUMBER', 'Existence in NAL collection', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('070', '$a', 'Classification number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('070', '$b', 'Item number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('070', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('071', 'NATIONAL AGRICULTURAL LIBRARY COPY STATEMENT', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('071', '$a', 'Classification number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('071', '$b', 'Item number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('071', '$c', 'Copy information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('071', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('072', 'SUBJECT CATEGORY CODE', 'Undefined', 'Source specified in subfield $2');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('072', '$a', 'Subject category code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('072', '$x', 'Subject category code subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('072', '$2', 'Source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('072', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('072', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('074', 'GPO ITEM NUMBER', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('074', '$a', 'GPO item number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('074', '$z', 'Canceled/invalid GPO item number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('074', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('080', 'UNIVERSAL DECIMAL CLASSIFICATION NUMBER', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('080', '$a', 'Universal Decimal Classification number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('080', '$b', 'Item number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('080', '$x', 'Common auxiliary subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('080', '$2', 'Edition identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('080', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('080', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('082', 'DEWEY DECIMAL CLASSIFICATION NUMBER', 'Type of edition', 'Source of classification number');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('082', '$a', 'Classification number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('082', '$b', 'Item number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('082', '$m', 'Standard or optional designation');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('082', '$q', 'Assigning agency');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('082', '$2', 'Edition number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('082', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('082', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('083', 'ADDITIONAL DEWEY DECIMAL CLASSIFICATION NUMBER', 'Type of edition', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('083', '$a', 'Classification number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('083', '$c', 'Classification number--Ending number of span');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('083', '$m', 'Standard or optional designation');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('083', '$q', 'Assigning agency');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('083', '$y', 'Table sequence number for internal subarrangement or add table');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('083', '$z', 'Table identification');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('083', '$2', 'Edition number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('083', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('083', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('084', 'OTHER CLASSIFICATION NUMBER', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('084', '$a', 'Classification number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('084', '$b', 'Item number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('084', '$2', 'Source of number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('084', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('084', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('085', 'SYNTHESIZED CLASSIFICATION NUMBER COMPONENTS', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$a', 'Number where instructions are found-single number or beginning number of span');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$b', 'Base number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$c', 'Classification number-ending number of span');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$f', 'Facet designator');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$r', 'Root number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$s', 'Digits added from classification number in schedule or external table');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$t', 'Digits added from internal subarrangement or add table');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$u', 'Number being analyzed');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$v', 'Number in internal subarrangement or add table where instructions are found');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$w', 'Table identification-Internal subarrangement or add table');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$y', 'Table sequence number for internal subarrangement or add table');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$z', 'Table identification');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('085', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('086', 'GOVERNMENT DOCUMENT CLASSIFICATION NUMBER', 'Number source', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('086', '$a', 'Classification number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('086', '$z', 'Canceled/invalid classification number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('086', '$2', 'Number source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('086', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('086', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('088', 'REPORT NUMBER', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('088', '$a', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('088', '$z', 'Canceled/invalid report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('088', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('088', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('100', 'MAIN ENTRY--PERSONAL NAME', 'Type of personal name entry element', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$a', 'Personal name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$b', 'Numeration ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$c', 'Titles and other words associated with a name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$d', 'Dates associated with a name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$e', 'Relator term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$j', 'Attribution qualifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$q', 'Fuller form of name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('100', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('110', 'MAIN ENTRY--CORPORATE NAME', 'Type of corporate name entry element', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$a', 'Corporate name or jurisdiction name as entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$b', 'Subordinate unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$c', 'Location of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$d', 'Date of meeting or treaty signing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$e', 'Relator term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$n', 'Number of part/section/meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('110', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('111', 'MAIN ENTRY--MEETING NAME', 'Type of meeting name entry element', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$a', 'Meeting name or jurisdiction name as entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$c', 'Location of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$d', 'Date of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$e', 'Subordinate unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$j', 'Relator term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$n', 'Number of part/section/meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$q', 'Name of meeting following jurisdiction name entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('111', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('130', 'MAIN ENTRY--UNIFORM TITLE', 'Nonfiling characters', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$a', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$d', 'Date of treaty signing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$m', 'Medium of performance for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$o', 'Arranged statement for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$r', 'Key for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('130', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('210', 'ABBREVIATED TITLE', 'Title added entry', 'Type');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('210', '$a', 'Abbreviated title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('210', '$b', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('210', '$2', 'Source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('210', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('210', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('222', 'KEY TITLE', 'Specifies whether variant title and/or added entry is required', 'Nonfiling characters');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('222', '$a', 'Key title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('222', '$b', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('222', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('222', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('240', 'UNIFORM TITLE', 'Uniform title printed or displayed', 'Nonfiling characters');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$a', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$d', 'Date of treaty signing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$m', 'Medium of performance for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$o', 'Arranged statement for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$r', 'Key for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('240', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('242', 'TRANSLATION OF TITLE BY CATALOGING AGENCY', 'Title added entry', 'Nonfiling characters');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('242', '$a', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('242', '$b', 'Remainder of title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('242', '$c', 'Statement of responsibility, etc. ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('242', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('242', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('242', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('242', '$y', 'Language code of translated title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('242', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('242', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('243', 'COLLECTIVE UNIFORM TITLE', 'Uniform title printed or displayed', 'Nonfiling characters');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$a', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$d', 'Date of treaty signing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$m', 'Medium of performance for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$o', 'Arranged statement for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$r', 'Key for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('243', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('245', 'TITLE STATEMENT', 'Title added entry', 'Nonfiling characters');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('245', '$a', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('245', '$b', 'Remainder of title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('245', '$c', 'Statement of responsibility, etc. ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('245', '$f', 'Inclusive dates ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('245', '$g', 'Bulk dates ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('245', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('245', '$k', 'Form ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('245', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('245', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('245', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('245', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('245', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('246', 'VARYING FORM OF TITLE', 'Note/added entry controller', 'Type of title');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('246', '$a', 'Title proper/short title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('246', '$b', 'Remainder of title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('246', '$f', 'Date or sequential designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('246', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('246', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('246', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('246', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('246', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('246', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('246', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('246', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('247', 'FORMER TITLE', 'Title added entry', 'Note controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('247', '$a', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('247', '$b', 'Remainder of title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('247', '$f', 'Date or sequential designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('247', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('247', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('247', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('247', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('247', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('247', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('247', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('250', 'EDITION STATEMENT', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('250', '$a', 'Edition statement ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('250', '$b', 'Remainder of edition statement ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('250', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('250', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('254', 'MUSICAL PRESENTATION STATEMENT', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('254', '$a', 'Musical presentation statement ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('254', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('254', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('255', 'CARTOGRAPHIC MATHEMATICAL DATA', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('255', '$a', 'Statement of scale ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('255', '$b', 'Statement of projection ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('255', '$c', 'Statement of coordinates ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('255', '$d', 'Statement of zone ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('255', '$e', 'Statement of equinox ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('255', '$f', 'Outer G-ring coordinate pairs ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('255', '$g', 'Exclusion G-ring coordinate pairs ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('255', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('255', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('256', 'COMPUTER FILE CHARACTERISTICS', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('256', '$a', 'Computer file characteristics ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('256', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('256', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('257', 'COUNTRY OF PRODUCING ENTITY FOR ARCHIVAL FILMS', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('257', '$a', 'Country of producing entity ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('257', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('257', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('258', 'PHILATELIC ISSUE DATE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('258', '$a', 'Issuing jurisdiction');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('258', '$b', 'Denomination');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('258', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('258', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('260', 'PUBLICATION, DISTRIBUTION, ETC. (IMPRINT)', 'Sequence of publishing statements', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('260', '$a', 'Place of publication, distribution, etc. ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('260', '$b', 'Name of publisher, distributor, etc. ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('260', '$c', 'Date of publication, distribution, etc. ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('260', '$d', 'Plate or publisher''s number for music (Pre-AACR 2)');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('260', '$e', 'Place of manufacture ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('260', '$f', 'Manufacturer ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('260', '$g', 'Date of manufacture ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('260', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('260', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('260', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('261', 'IMPRINT STATEMENT FOR FILMS (Pre-AACR 1 Revised)', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('261', '$a', 'Producing company ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('261', '$b', 'Releasing company (primary distributor) ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('261', '$d', 'Date of production, release, etc. ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('261', '$e', 'Contractual producer ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('261', '$f', 'Place of production, release, etc. ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('261', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('261', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('262', 'IMPRINT STATEMENT FOR SOUND RECORDINGS (Pre-AACR 2)', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('262', '$a', 'Place of production, release, etc. ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('262', '$b', 'Publisher or trade name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('262', '$c', 'Date of production, release, etc. ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('262', '$k', 'Serial identification ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('262', '$l', 'Matrix and/or take number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('262', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('262', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('263', 'PROJECTED PUBLICATION DATE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('263', '$a', 'Projected publication date ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('263', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('263', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('264', 'PRODUCTION, PUBLICATION, DISTRIBUTION, MANUFACTURE, AND COPYRIGHT NOTICE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('264', '$a', 'Place of production, publication, distribution, manufacture');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('264', '$b', 'Name of producer, publisher, distributor, manufacturer');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('264', '$c', 'Date of production, publication, distribution, manufacture, or copyright notice');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('264', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('264', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('264', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('270', 'ADDRESS', 'Level', 'Type of address');



INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$b', 'City ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$c', 'State or province ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$d', 'Country ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$e', 'Postal code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$f', 'Terms preceding attention name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$g', 'Attention name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$h', 'Attention position ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$i', 'Type of address ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$j', 'Specialized telephone number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$k', 'Telephone number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$l', 'Fax number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$m', 'Electronic mail address ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$n', 'TDD or TTY number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$p', 'Contact person ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$q', 'Title of contact person ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$r', 'Hours ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$z', 'Public note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('270', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('300', 'PHYSICAL DESCRIPTION', 'Undefined', 'Undefined');



INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('300', '$b', 'Other physical details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('300', '$c', 'Dimensions ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('300', '$e', 'Accompanying material ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('300', '$f', 'Type of unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('300', '$g', 'Size of unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('300', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('300', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('300', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('306', 'PLAYING TIME', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('306', '$a', 'Playing time ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('306', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('306', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('307', 'HOURS, ETC.', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('307', '$a', 'Hours ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('307', '$b', 'Additional information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('307', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('307', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('310', 'CURRENT PUBLICATION FREQUENCY', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('310', '$a', 'Current publication frequency ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('310', '$b', 'Date of current publication frequency ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('310', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('310', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('321', 'FORMER PUBLICATION FREQUENCY', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('321', '$a', 'Former publication frequency ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('321', '$b', 'Dates of former publication frequency ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('321', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('321', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('336', 'CONTENT TYPE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('336', '$a', 'Content type term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('0', '$b', 'Content type code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('0', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('336', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('0', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('0', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('337', 'MEDIA TYPE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('337', '$a', 'Media type term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('337', '$b', 'Media type code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('337', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('337', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('337', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('337', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('338', 'CARRIER TYPE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('338', '$a', 'Carrier type term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('338', '$b', 'Carrier type code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('338', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('338', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('338', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('338', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('340', 'PHYSICAL MEDIUM', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('340', '$a', 'Material base and configuration ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('340', '$b', 'Dimensions ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('340', '$c', 'Materials applied to surface ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('340', '$d', 'Information recording technique ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('340', '$e', 'Support ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('340', '$f', 'Production rate/ratio ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('340', '$h', 'Location within medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('340', '$i', 'Technical specifications of medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('340', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('340', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('340', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('342', 'GEOSPATIAL REFERENCE DATA', 'Geospatial reference dimension', 'Geospatial reference method');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$a', 'Name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$b', 'Coordinate or distance units ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$c', 'Latitude resolution ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$d', 'Longitude resolution ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$e', 'Standard parallel or oblique line latitude ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$f', 'Oblique line longitude ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$g', 'Longitude of central meridian or projection center ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$h', 'Latitude of projection origin or projection center ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$i', 'False easting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$j', 'False northing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$k', 'Scale factor ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$l', 'Height of perspective point above surface ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$m', 'Azimuthal angle ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$o', 'Landsat number and path number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$p', 'Zone identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$q', 'Ellipsoid name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$r', 'Semi-major axis ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$s', 'Denominator of flattening ratio ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$t', 'Vertical resolution ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$u', 'Vertical encoding method ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$v', 'Local planar, local, or other projection or grid description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$w', 'Local planar or local georeference information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$2', 'Reference method used ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('342', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('343', 'PLANAR COORDINATE DATA', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('343', '$a', 'Planar coordinate encoding method ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('343', '$b', 'Planar distance units ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('343', '$c', 'Abscissa resolution ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('343', '$d', 'Ordinate resolution ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('343', '$e', 'Distance resolution ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('343', '$f', 'Bearing resolution ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('343', '$g', 'Bearing units ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('343', '$h', 'Bearing reference direction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('343', '$i', 'Bearing reference meridian ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('343', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('343', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('344', 'SOUND CHARACTERISTICS', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$a', 'Type of recording');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$b', 'Recording medium');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$c', 'Playing speed');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$d', 'Groove characteristics');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$e', 'Track configuration');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$f', 'Tape configuration');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$g', 'Configuration of playback channels');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$h', 'Special playback characteristics');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$0', 'Authority record control number or standard number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('344', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('345', 'PROJECTION CHARACTERISTICS OF MOVING IMAGE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('345', '$a', 'Presentation format');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('345', '$b', 'Projection speed');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('345', '$0', 'Authority record control number or standard number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('345', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('345', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('345', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('345', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('346', 'VIDEO CHARACTERISTICS', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('346', '$a', 'Video format');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('346', '$b', 'Broadcast standard');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('346', '$0', 'Authority record control number or standard number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('346', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('346', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('346', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('346', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('347', 'DIGITAL FILE CHARACTERISTICS', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('347', '$a', 'File type');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('347', '$b', 'Encoding format');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('347', '$c', 'File size');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('347', '$d', 'Resolution');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('347', '$e', 'Regional encoding');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('347', '$f', 'Transmission speed');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('347', '$0', 'Authority record control number or standard number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('347', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('347', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('347', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('347', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('351', 'ORGANIZATION AND ARRANGEMENT OF MATERIALS', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('351', '$a', 'Organization ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('351', '$b', 'Arrangement ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('351', '$c', 'Hierarchical level ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('351', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('351', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('351', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('352', 'DIGITAL GRAPHIC REPRESENTATION', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('352', '$a', 'Direct reference method ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('352', '$b', 'Object type ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('352', '$c', 'Object count ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('352', '$d', 'Row count ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('352', '$e', 'Column count ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('352', '$f', 'Vertical count ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('352', '$g', 'VPF topology level ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('352', '$i', 'Indirect reference description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('352', '$q', 'Format of the digital image ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('352', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('352', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('355', 'SECURITY CLASSIFICATION CONTROL', 'Controlled element', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('355', '$a', 'Security classification ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('355', '$b', 'Handling instructions ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('355', '$c', 'External dissemination information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('355', '$d', 'Downgrading or declassification event ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('355', '$e', 'Classification system ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('355', '$f', 'Country of origin code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('355', '$g', 'Downgrading date ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('355', '$h', 'Declassification date ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('355', '$j', 'Authorization ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('355', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('355', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('357', 'ORIGINATOR DISSEMINATION CONTROL', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('357', '$a', 'Originator control term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('357', '$b', 'Originating agency ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('357', '$c', 'Authorized recipients of material ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('357', '$g', 'Other restrictions ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('357', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('357', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('362', 'DATES OF PUBLICATION AND/OR SEQUENTIAL DESIGNATION', 'Format of date', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('362', '$a', 'Dates of publication and/or sequential designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('362', '$z', 'Source of information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('362', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('362', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('363', 'NORMALIZED DATE AND SEQUENTIAL DESIGNATION', 'Start/End designator', 'State of issuance');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$a', 'First level of enumeration');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$b', 'Second level of enumeration');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$c', 'Third level of enumeration');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$d', 'Fourth level of enumeration');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$e', 'Fifth level of enumeration');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$f', 'Sixth level of enumeration');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$g', 'Alternative numbering scheme, first level of enumeration');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$h', 'Alternative numbering scheme, second level of enumeration');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$i', 'First level of chronology');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$j', 'Second level of chronology');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$k', 'Third level of chronology');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$l', 'Fourth level of chronology');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$m', 'Alternative numbering scheme, chronology');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$u', 'First level textual designation');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$v', 'First level of chronology, issuance');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$x', 'Nonpublic note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$z', 'Public note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('363', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('365', 'TRADE PRICE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$a', 'Price type code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$b', 'Price amount ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$c', 'Currency code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$d', 'Unit of pricing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$e', 'Price note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$f', 'Price effective from ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$g', 'Price effective until ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$h', 'Tax rate 1 ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$i', 'Tax rate 2 ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$j', 'ISO country code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$k', 'MARC country code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$m', 'Identification of pricing entity ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$2', 'Source of price type code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('365', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('366', 'TRADE AVAILABILITY INFORMATION', 'Undefined', 'Undefined');

INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$a', 'Publishers'' compressed title identification ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$b', 'Detailed date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$c', 'Availability status code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$d', 'Expected next availability date ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$e', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$f', 'Publishers'' discount category ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$g', 'Date made out of print ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$j', 'ISO country code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$k', 'MARC country code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$m', 'Identification of agency ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$2', 'Source of availability status code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('366', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('377', 'ASSOCIATED LANGUAGE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('377', '$a', 'Language code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('377', '$l', 'Language term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('377', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('377', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('377', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('380', 'FORM OF WORK', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('380', '$a', 'Form of work');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('380', '$0', 'Record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('380', '$2', 'Source of term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('380', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('380', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('381', 'OTHER DISTINGUISHING CHARACTERISTICS OF WORK OR EXPRESSION', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('381', '$a', 'Other distinguishing characteristics');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('381', '$u', 'Uniform Resource Identifier');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('381', '$v', 'Source of information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('381', '$0', 'Record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('381', '$2', 'Source of term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('381', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('381', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('382', 'MEDIUM OF PERFORMANCE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('382', '$a', 'Medium of performance');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('382', '$b', 'Soloist');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('382', '$d', 'Doubling instrument');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('382', '$n', 'Number of performers of the same medium');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('382', '$p', 'Alternative medium performance');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('382', '$s', 'Total number of performers');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('382', '$v', 'Note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('382', '$0', 'Authority record control number or standard number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('382', '$2', 'Source of term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('382', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('382', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('383', 'NUMERIC DESIGNATION OF MUSICAL WORK', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('383', '$a', 'Serial number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('383', '$b', 'Opus number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('383', '$c', 'Thematic index number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('383', '$d', 'Thematic index code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('383', '$e', 'Publisher associated with opus number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('383', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('383', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('383', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('384', 'KEY', 'Key type', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('384', '$a', 'Key');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('384', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('384', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('385', 'AUDIENCE CHARACTERISTICS', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('385', '$a', 'Audience term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('385', '$b', 'Audience code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('385', '$m', 'Demographic group term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('385', '$n', 'Demographic group code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('385', '$0', 'Authority record control number or standard number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('385', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('385', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('385', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('385', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('386', 'CREATOR/CONTRIBUTOR CHARACTERISTICS', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('386', '$a', 'Creator/contributor term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('386', '$b', 'Creator/contributor code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('386', '$m', 'Demographic group term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('386', '$n', 'Demographic group code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('386', '$0', 'Authority record control number or standard number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('386', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('386', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('386', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('386', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('400', 'SERIES STATEMENT/ADDED ENTRY--PERSONAL NAME ', 'Type of personal name entry element', 'Pronoun represents main entry');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$a', 'Personal name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$b', 'Numeration ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$c', 'Titles and other words associated with a name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$d', 'Dates associated with a name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$e', 'Relator term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$v', 'Volume number/sequential designation  ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$x', 'International Standard Serial Number  ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('400', '$8', 'Field link and sequence number  ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('410', 'SERIES STATEMENT/ADDED ENTRY--CORPORATE NAME', 'Type of corporate name entry element', 'Pronoun represents main entry');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$a', 'Corporate name or jurisdiction name as entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$b', 'Subordinate unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$c', 'Location of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$d', 'Date of meeting or treaty signing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$e', 'Relator term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$n', 'Number of part/section/meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$v', 'Volume number/sequential designation  ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('410', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('411', 'SERIES STATEMENT/ADDED ENTRY--MEETING NAME', 'Type of meeting name entry element', 'Pronoun represents main entry');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$a', 'Meeting name or jurisdiction name as entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$c', 'Location of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$d', 'Date of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$e', 'Subordinate unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$n', 'Number of part/section/meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$q', 'Name of meeting following jurisdiction name entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$v', 'Volume number/sequential designation  ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('411', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('440', 'SERIES STATEMENT/ADDED ENTRY--TITLE [OBSOLETE]', 'Undefined', 'Nonfiling characters');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('440', '$a', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('440', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('440', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('440', '$v', 'Volume number/sequential designation  ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('440', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('440', '$w', 'Bibliographic record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('440', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('440', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('440', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('490', 'SERIES STATEMENT', 'Specifies whether series is traced', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('490', '$a', 'Series statement ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('490', '$l', 'Library of Congress call number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('490', '$v', 'Volume number/sequential designation  ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('490', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('490', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('490', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('490', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('500', 'GENERAL NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('500', '$a', 'General note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('500', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('500', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('500', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('500', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('501', 'WITH NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('501', '$a', 'With note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('501', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('501', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('501', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('502', 'DISSERTATION NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('502', '$a', 'Dissertation note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('502', '$b', 'Degree type');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('502', '$c', 'Name of granting institution');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('502', '$d', 'Year of degree granted');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('502', '$g', 'Miscellaneous information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('502', '$o', 'Dissertation identifier');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('502', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('502', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('504', 'BIBLIOGRAPHY, ETC. NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('504', '$a', 'Bibliography, etc. note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('504', '$b', 'Number of references ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('504', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('504', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('505', 'FORMATTED CONTENTS NOTE', 'Display constant controller', 'Level of content designation');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('505', '$a', 'Formatted contents note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('505', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('505', '$r', 'Statement of responsibility ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('505', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('505', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('505', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('505', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('506', 'RESTRICTIONS ON ACCESS NOTE', 'Restriction', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('506', '$a', 'Terms governing access ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('506', '$b', 'Jurisdiction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('506', '$c', 'Physical access provisions ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('506', '$d', 'Authorized users ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('506', '$e', 'Authorization ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('506', '$f', 'Standard terminology for access restiction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('506', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('506', '$2', 'Source of term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('506', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('506', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('506', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('506', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('507', 'SCALE NOTE FOR GRAPHIC MATERIAL', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('507', '$a', 'Representative fraction of scale note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('507', '$b', 'Remainder of scale note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('507', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('507', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('508', 'CREATION/PRODUCTION CREDITS NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('508', '$a', 'Creation/production credits note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('508', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('508', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('510', 'CITATION/REFERENCES NOTE', 'Coverage/location in source', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('510', '$a', 'Name of source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('510', '$b', 'Coverage of source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('510', '$c', 'Location within source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('510', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('510', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('510', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('510', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('511', 'PARTICIPANT OR PERFORMER NOTE', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('511', '$a', 'Participant or performer note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('511', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('511', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('513', 'TYPE OF REPORT AND PERIOD COVERED NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('513', '$a', 'Type of report ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('513', '$b', 'Period covered ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('513', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('513', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('514', 'DATA QUALITY NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$a', 'Attribute accuracy report ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$b', 'Attribute accuracy value ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$c', 'Attribute accuracy explanation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$d', 'Logical consistency report ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$e', 'Completeness report ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$f', 'Horizontal position accuracy report ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$g', 'Horizontal position accuracy value ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$h', 'Horizontal position accuracy explanation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$i', 'Vertical positional accuracy report ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$j', 'Vertical positional accuracy value ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$k', 'Vertical positional accuracy explanation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$m', 'Cloud cover ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$z', 'Display note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('514', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('515', 'NUMBERING PECULIARITIES NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('515', '$a', 'Numbering peculiarities note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('515', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('515', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('516', 'TYPE OF COMPUTER FILE OR DATA NOTE', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('516', '$a', 'Type of computer file or data note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('516', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('516', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('518', 'DATE/TIME AND PLACE OF AN EVENT NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('518', '$a', 'Date/time and place of an event note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('518', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('518', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('518', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('520', 'SUMMARY, ETC.', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('520', '$a', 'Summary, etc. note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('520', '$b', 'Expansion of summary note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('520', '$c', 'Assigning agency');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('520', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('520', '$2', 'Source');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('520', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('520', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('520', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('521', 'TARGET AUDIENCE NOTE', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('521', '$a', 'Target audience note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('521', '$b', 'Source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('521', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('521', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('521', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('522', 'GEOGRAPHIC COVERAGE NOTE', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('522', '$a', 'Geographic coverage note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('0', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('0', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('524', 'PREFERRED CITATION OF DESCRIBED MATERIALS NOTE', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('524', '$a', 'Preferred citation of described materials note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('524', '$2', 'Source of schema used ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('524', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('524', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('524', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('525', 'SUPPLEMENT NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('525', '$a', 'Supplement note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('525', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('525', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('526', 'STUDY PROGRAM INFORMATION NOTE', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('526', '$a', 'Program name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('526', '$b', 'Interest level ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('526', '$c', 'Reading level ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('526', '$d', 'Title point value ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('526', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('526', '$x', 'Nonpublic note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('526', '$z', 'Public note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('526', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('526', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('526', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('530', 'ADDITIONAL PHYSICAL FORM AVAILABLE NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('530', '$a', 'Additional physical form available note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('530', '$b', 'Availability source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('530', '$c', 'Availability conditions ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('530', '$d', 'Order number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('530', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('530', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('530', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('530', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('533', 'REPRODUCTION NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$a', 'Type of reproduction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$b', 'Place of reproduction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$c', 'Agency responsible for reproduction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$d', 'Date of reproduction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$e', 'Physical description of reproduction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$f', 'Series statement of reproduction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$m', 'Dates and/or sequential designation of issues reproduced ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$n', 'Note about reproduction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$5', 'Institution to which field applies');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$7', 'Fixed-length data elements of reproduction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('533', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('534', 'ORIGINAL VERSION NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$a', 'Main entry of original ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$b', 'Edition statement of original ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$c', 'Publication, distribution, etc. of original ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$e', 'Physical description, etc. of original ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$f', 'Series statement of original ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$k', 'Key title of original ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$l', 'Location of original ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$m', 'Material specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$n', 'Note about original ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$o', 'Other resource identifier');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$p', 'Introductory phrase ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$t', 'Title statement of original ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('534', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('535', 'LOCATION OF ORIGINALS/DUPLICATES NOTE', 'Additional information about custodian', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('535', '$a', 'Custodian ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('535', '$b', 'Postal address ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('535', '$c', 'Country ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('535', '$d', 'Telecommunications address ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('535', '$g', 'Repository location code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('535', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('535', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('535', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('536', 'FUNDING INFORMATION NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('536', '$a', 'Text of note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('536', '$b', 'Contract number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('536', '$c', 'Grant number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('536', '$d', 'Undifferentiated number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('536', '$e', 'Program element number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('536', '$f', 'Project number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('536', '$g', 'Task number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('536', '$h', 'Work unit number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('536', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('536', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('538', 'SYSTEM DETAILS NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('538', '$a', 'System details note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('538', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('538', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('538', '$3', 'Materials specified  ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('538', '$5', 'Institution to which field applies');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('538', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('538', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('540', 'TERMS GOVERNING USE AND REPRODUCTION NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('540', '$a', 'Terms governing use and reproduction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('540', '$b', 'Jurisdiction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('540', '$c', 'Authorization ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('540', '$d', 'Authorized users ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('540', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('540', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('540', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('540', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('540', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('541', 'IMMEDIATE SOURCE OF ACQUISITION NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$a', 'Source of acquisition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$b', 'Address ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$c', 'Method of acquisition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$d', 'Date of acquisition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$e', 'Accession number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$f', 'Owner ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$h', 'Purchase price ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$n', 'Extent ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$o', 'Type of unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('541', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('542', 'INFORMATION RELATING TO COPYRIGHT STATUS', 'Relationship', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$a', 'Personal creator');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$b', 'Personal creator death date');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$c', 'Corporate creator');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$d', 'Copyright holder');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$e', 'Copyright holder contact information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$f', 'Copyright statement');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$g', 'Copyright date');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$h', 'Copyright renewal date');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$i', 'Publication date');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$j', 'Creation date');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$k', 'Publisher');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$l', 'Copyright status');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$m', 'Publication status');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$n', 'Note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$o', 'Research date');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$q', 'Assigning agency');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$r', 'Jurisdiction of copyright assessment');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$s', 'Source of information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$u', 'Uniform Resource Identifier');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('542', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('544', 'LOCATION OF OTHER ARCHIVAL MATERIALS NOTE', 'Relationship', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('544', '$a', 'Custodian ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('544', '$b', 'Address ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('544', '$c', 'Country ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('544', '$d', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('544', '$e', 'Provenance ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('544', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('544', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('544', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('544', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('545', 'BIOGRAPHICAL OR HISTORICAL DATA', 'Type of data', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('545', '$a', 'Biographical or historical note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('545', '$b', 'Expansion ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('545', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('545', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('545', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('546', 'LANGUAGE NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('546', '$a', 'Language note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('546', '$b', 'Information code or alphabet ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('546', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('546', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('546', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('547', 'FORMER TITLE COMPLEXITY NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('547', '$a', 'Former title complexity note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('547', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('547', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('550', 'ISSUING BODY NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('550', '$a', 'Issuing body note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('550', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('550', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('552', 'ENTITY AND ATTRIBUTE INFORMATION NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$a', 'Entity type label ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$b', 'Entity type definition and source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$c', 'Attribute label ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$d', 'Attribute definition and source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$e', 'Enumerated domain value ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$f', 'Enumerated domain value definition and source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$g', 'Range domain minimum and maximum ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$h', 'Codeset name and source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$i', 'Unrepresentable domain ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$j', 'Attribute units of measurement and resolution ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$k', 'Beginning date and ending date of attribute values ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$l', 'Attribute value accuracy ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$m', 'Attribute value accuracy explanation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$n', 'Attribute measurement frequency ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$o', 'Entity and attribute overview ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$p', 'Entity and attribute detail citation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$z', 'Display note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('552', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('555', 'CUMULATIVE INDEX/FINDING AIDS NOTE', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('555', '$a', 'Cumulative index/finding aids note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('555', '$b', 'Availability source ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('555', '$c', 'Degree of control ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('555', '$d', 'Bibliographic reference ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('555', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('555', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('555', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('555', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('556', 'INFORMATION ABOUT DOCUMENTATION NOTE', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('556', '$a', 'Information about documentation note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('556', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('556', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('556', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('561', 'OWNERSHIP AND CUSTODIAL HISTORY', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('561', '$a', 'History ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('561', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('561', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('561', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('561', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('562', 'COPY AND VERSION IDENTIFICATION NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('562', '$a', 'Identifying markings ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('562', '$b', 'Copy identification ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('562', '$c', 'Version identification ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('562', '$d', 'Presentation format ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('562', '$e', 'Number of copies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('562', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('562', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('562', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('562', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('563', 'BINDING INFORMATION', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('563', '$a', 'Binding note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('563', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('563', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('563', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('563', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('563', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('565', 'CASE FILE CHARACTERISTICS NOTE', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('565', '$a', 'Number of cases/variables ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('565', '$b', 'Name of variable ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('565', '$c', 'Unit of analysis ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('565', '$d', 'Universe of data ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('565', '$e', 'Filing scheme or code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('565', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('565', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('565', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('567', 'METHODOLOGY NOTE', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('567', '$a', 'Methodology note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('567', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('567', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('580', 'LINKING ENTRY COMPLEXITY NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('580', '$a', 'Linking entry complexity note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('580', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('580', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('581', 'PUBLICATIONS ABOUT DESCRIBED MATERIALS NOTE', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('581', '$a', 'Publications about described materials note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('581', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('581', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('581', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('581', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('583', 'ACTION NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$a', 'Action ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$b', 'Action identification ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$c', 'Time/date of action ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$d', 'Action interval ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$e', 'Contingency for action ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$f', 'Authorization ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$h', 'Jurisdiction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$i', 'Method of action ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$j', 'Site of action ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$k', 'Action agent ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$l', 'Status ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$n', 'Extent ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$o', 'Type of unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$x', 'Nonpublic note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$z', 'Public note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$2', 'Source of term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('583', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('584', 'ACCUMULATION AND FREQUENCY OF USE NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('584', '$a', 'Accumulation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('584', '$b', 'Frequency of use ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('584', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('584', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('584', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('584', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('585', 'EXHIBITIONS NOTE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('585', '$a', 'Exhibitions note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('585', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('585', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('585', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('585', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('586', 'AWARDS NOTE', 'Display constant controller', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('586', '$a', 'Awards note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('586', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('586', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('586', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('600', 'SUBJECT ADDED ENTRY--PERSONAL NAME', 'Type of personal name entry element', 'Thesaurus');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$a', 'Personal name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$b', 'Numeration ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$c', 'Titles and other words associated with a name');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$d', 'Dates associated with a name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$e', 'Relator term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$j', 'Attribution qualifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$m', 'Medium of performance for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$o', 'Arranged statement for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$q', 'Fuller form of name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$r', 'Key for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$v', 'Form subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$x', 'General subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$y', 'Chronological subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$z', 'Geographic subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$2', 'Source of heading or term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('600', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('610', 'SUBJECT ADDED ENTRY--CORPORATE NAME', 'Type of corporate name entry element', 'Thesaurus');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$a', 'Corporate name or jurisdiction name as entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$b', 'Subordinate unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$c', 'Location of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$d', 'Date of meeting or treaty signing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$e', 'Relator term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$m', 'Medium of performance for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$n', 'Number of part/section/meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$o', 'Arranged statement for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$r', 'Key for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$v', 'Form subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$x', 'General subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$y', 'Chronological subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$z', 'Geographic subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$2', 'Source of heading or term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('610', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('611', 'SUBJECT ADDED ENTRY--MEETING NAME', 'Type of meeting name entry element', 'Thesaurus');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$a', 'Meeting name or jurisdiction name as entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$c', 'Location of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$d', 'Date of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$e', 'Subordinate unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$j', 'Relator term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$n', 'Number of part/section/meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$q', 'Name of meeting following jurisdiction name entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$v', 'Form subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$x', 'General subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$y', 'Chronological subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$z', 'Geographic subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$2', 'Source of heading or term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('611', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('630', 'SUBJECT ADDED ENTRY--UNIFORM TITLE', 'Nonfiling characters', 'Thesaurus');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$a', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$d', 'Date of treaty signing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$e', 'Relator term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$m', 'Medium of performance for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$o', 'Arranged statement for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$r', 'Key for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$v', 'Form subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$x', 'General subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$y', 'Chronological subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$z', 'Geographic subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$2', 'Source of heading or term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$4', 'Relator code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('630', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('648', 'SUBJECT ADDED ENTRY--CHRONOLOGICAL TERM', 'Undefined', 'Thesaurus');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('648', '$a', 'Chronological term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('648', '$v', 'Form subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('648', '$x', 'General subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('648', '$y', 'Chronological subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('648', '$z', 'Geographic subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('648', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('648', '$2', 'Source of heading or term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('648', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('648', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('648', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('650', 'SUBJECT ADDED ENTRY--TOPICAL TERM', 'Level of subject', 'Thesaurus');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$a', 'Topical term or geographic name as entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$b', 'Topical term following geographic name as entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$c', 'Location of event ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$d', 'Active dates ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$e', 'Relator term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$v', 'Form subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$x', 'General subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$y', 'Chronological subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$z', 'Geographic subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$2', 'Source of heading or term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$4', 'Relator code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('650', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('651', 'SUBJECT ADDED ENTRY--GEOGRAPHIC NAME', 'Undefined', 'Thesaurus');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('651', '$a', 'Geographic name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('651', '$e', 'Relator term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('651', '$v', 'Form subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('651', '$x', 'General subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('651', '$y', 'Chronological subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('651', '$z', 'Geographic subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('651', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('651', '$2', 'Source of heading or term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('651', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('651', '$4', 'Relator code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('651', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('651', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('653', 'INDEX TERM--UNCONTROLLED', 'Level of index term', 'Type of term or name');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('653', '$a', 'Uncontrolled term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('653', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('653', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('654', 'SUBJECT ADDED ENTRY--FACETED TOPICAL TERMS', 'Level of subject', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$a', 'Focus term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$b', 'Non-focus term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$c', 'Facet/hierarchy designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$e', 'Relator term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$v', 'Form subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$y', 'Chronological subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$z', 'Geographic subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$2', 'Source of heading or term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$4', 'Relator code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('654', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('655', 'INDEX TERM--GENRE/FORM', 'Type of heading', 'Thesaurus');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$a', 'Genre/form data or focus term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$b', 'Non-focus term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$c', 'Facet/hierarchy designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$v', 'Form subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$x', 'General subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$y', 'Chronological subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$z', 'Geographic subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$2', 'Source of term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('655', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('656', 'INDEX TERM--OCCUPATION', 'Undefined', 'Source of term');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('656', '$a', 'Occupation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('656', '$k', 'Form ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('656', '$v', 'Form subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('656', '$x', 'General subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('656', '$y', 'Chronological subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('656', '$z', 'Geographic subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('656', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('656', '$2', 'Source of term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('656', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('656', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('656', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('657', 'INDEX TERM--FUNCTION', 'Undefined', 'Source of term');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('657', '$a', 'Function ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('657', '$v', 'Form subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('657', '$x', 'General subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('657', '$y', 'Chronological subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('657', '$z', 'Geographic subdivision ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('657', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('657', '$2', 'Source of term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('657', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('657', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('657', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('658', 'INDEX TERM--CURRICULUM OBJECTIVE', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('658', '$a', 'Main curriculum objective ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('658', '$b', 'Subordinate curriculum objective ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('658', '$c', 'Curriculum code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('658', '$d', 'Correlation factor ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('658', '$2', 'Source of term or code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('658', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('658', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('662', 'SUBJECT ADDED ENTRY--HIERARCHICAL PLACE NAME', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$a', 'Country or larger entity');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$b', 'First-order political jurisdiction');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$c', 'Intermediate political jurisdiction');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$d', 'City');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$e', 'Relator term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$f', 'City subsection');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$g', 'Other nonjurisdictional geographic region and feature');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$h', 'Extraterrestrial area');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$2', 'Source of heading or term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$4', 'Relator code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('662', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('700', 'ADDED ENTRY--PERSONAL NAME', 'Type of personal name entry element', 'Type of added entry');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$a', 'Personal name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$b', 'Numeration ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$c', 'Titles and other words associated with a name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$d', 'Dates associated with a name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$e', 'Relator term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$j', 'Attribution qualifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$m', 'Medium of performance for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$o', 'Arranged statement for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$q', 'Fuller form of name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$r', 'Key for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('700', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('710', 'ADDED ENTRY--CORPORATE NAME', 'Type of corporate name entry element', 'Type of added entry');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$a', 'Corporate name or jurisdiction name as entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$b', 'Subordinate unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$c', 'Location of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$d', 'Date of meeting or treaty signing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$e', 'Relator term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$m', 'Medium of performance for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$n', 'Number of part/section/meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$o', 'Arranged statement for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$r', 'Key for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('710', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('711', 'ADDED ENTRY--MEETING NAME', 'Type of meeting name entry element', 'Type of added entry');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$a', 'Meeting name or jurisdiction name as entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$c', 'Location of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$d', 'Date of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$e', 'Subordinate unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$j', 'Relator term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$n', 'Number of part/section/meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$q', 'Name of meeting following jurisdiction name entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('711', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('720', 'ADDED ENTRY--UNCONTROLLED NAME', 'Type of name', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('720', '$a', 'Name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('720', '$e', 'Relator term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('720', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('720', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('720', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('730', 'ADDED ENTRY--UNIFORM TITLE', 'Nonfiling characters', 'Type of added entry');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$a', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$d', 'Date of treaty signing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$m', 'Medium of performance for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$o', 'Arranged statement for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$r', 'Key for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('730', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('740', 'ADDED ENTRY--UNCONTROLLED RELATED/ANALYTICAL TITLE', 'Nonfiling characters', 'Type of added entry');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('740', '$a', 'Uncontrolled related/analytical title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('740', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('740', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('740', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('740', '$5', 'Institution to which field applies ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('740', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('740', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('751', 'ADDED ENTRY--GEOGRAPHIC NAME', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('751', '$a', 'Geographic name');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('751', '$e', 'Relator term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('751', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('751', '$2', 'Source of heading or term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('751', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('751', '$4', 'Relator code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('751', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('751', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('752', 'ADDED ENTRY--HIERARCHICAL PLACE NAME', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('752', '$a', 'Country or larger entity');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('752', '$b', 'First-order political jurisdiction');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('752', '$c', 'Intermediate political jurisdiction');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('752', '$d', 'City');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('752', '$f', 'City subsection');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('752', '$g', 'Other nonjurisdictional geographic region and feature');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('752', '$h', 'Extraterrestrial area');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('752', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('752', '$2', 'Source of heading or term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('752', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('752', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('753', 'SYSTEM DETAILS ACCESS TO COMPUTER FILES', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('753', '$a', 'Make and model of machine ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('753', '$b', 'Programming language ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('753', '$c', 'Operating system ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('753', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('753', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('754', 'ADDED ENTRY--TAXONOMIC IDENTIFICATION', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('754', '$a', 'Taxonomic name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('754', '$c', 'Taxonomic category ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('754', '$d', 'Common or alternative name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('754', '$x', 'Non-public note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('754', '$z', 'Public note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('754', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('754', '$2', 'Source of taxonomic identification ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('754', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('754', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('760', 'MAIN SERIES ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('760', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('762', 'SUBSERIES ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('762', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('765', 'ORIGINAL LANGUAGE ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$r', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$u', 'Standard Technical Report Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('765', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('767', 'TRANSLATION ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$r', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$u', 'Standard Technical Report Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('767', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('770', 'SUPPLEMENT/SPECIAL ISSUE ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$r', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$u', 'Standard Technical Report Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('770', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('772', 'SUPPLEMENT PARENT ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$r', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$u', 'Standard Technical Report Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$z', 'International Stan dard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('772', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('773', 'HOST ITEM ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$p', 'Abbreviated title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$q', 'Enumeration and first page ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$r', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$u', 'Standard Technical Report Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('773', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('774', 'CONSTITUENT UNIT ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$r', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$u', 'Standard Technical Report Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('774', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('775', 'OTHER EDITION ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$e', 'Language code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$f', 'Country code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$r', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$u', 'Standard Technical Report Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('775', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('776', 'ADDITIONAL PHYSICAL FORM ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$r', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$u', 'Standard Technical Report Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('776', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('777', 'ISSUED WITH ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('777', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('780', 'PRECEDING ENTRY', 'Note controller', 'Type of relationship');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$r', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$u', 'Standard Technical Report Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('780', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('785', 'SUCCEEDING ENTRY', 'Note controller', 'Type of relationship');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$r', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$u', 'Standa rd Technical Report Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('785', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('786', 'DATA SOURCE ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$j', 'Period of content ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$p', 'Abbreviated title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$r', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$u', 'Standard Technical Report Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$v', 'Source Contribution ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('786', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('787', 'NONSPECIFIC RELATIONSHIP ENTRY', 'Note controller', 'Display constant controller');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$a', 'Main entry heading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$b', 'Edition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$c', 'Qualifying information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$d', 'Place, publisher, and date of publication ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$g', 'Relationship information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$h', 'Physical description ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$i', 'Display text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$k', 'Series data for related item ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$m', 'Material-specific details ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$n', 'Note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$o', 'Other item identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$r', 'Report number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$s', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$t', 'Title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$u', 'Standard Technical Report Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$x', 'International Standard Serial Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$y', 'CODEN designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$z', 'International Standard Book Number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$7', 'Control subfield ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('787', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('800', 'SERIES ADDED ENTRY--PERSONAL NAME', 'Type of personal name entry element', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$a', 'Personal name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$b', 'Numeration ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$c', 'Titles and other words associated with a name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$d', 'Dates associated with a name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$e', 'Relator term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$j', 'Attribution qualifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$m', 'Medium of performance for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$n', 'Number of part/section of a work  ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$o', 'Arranged statement for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$q', 'Fuller form of name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$r', 'Key for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$v', 'Volume/sequential designation  ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$w', 'Bibliographic record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$x', 'International Standard Serial Number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('800', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('810', 'SERIES ADDED ENTRY--CORPORATE NAME', 'Type of corporate name entry element', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$a', 'Corporate name or jurisdiction name as entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$b', 'Subordinate unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$c', 'Location of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$d', 'Date of meeting or treaty signing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$e', 'Relator term ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$m', 'Medium of performance for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$n', 'Number of part/section/meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$o', 'Arranged statement for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$r', 'Key for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$v', 'Volume/sequential designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$w', 'Bibliographic record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$x', 'International Standard Serial Number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('810', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('811', 'SERIES ADDED ENTRY--MEETING NAME', 'Type of meeting name entry element', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$a', 'Meeting name or jurisdiction name as entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$c', 'Location of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$d', 'Date of meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$e', 'Subordinate unit ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$j', 'Relator term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$n', 'Number of part/section/meeting ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$q', 'Name of meeting following jurisdiction name entry element ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$u', 'Affiliation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$v', 'Volume/sequential designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$w', 'Bibliographic record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$x', 'International Standard Serial Number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$4', 'Relator code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('811', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('830', 'SERIES ADDED ENTRY--UNIFORM TITLE', 'Undefined', 'Nonfiling characters');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$a', 'Uniform title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$d', 'Date of treaty signing ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$f', 'Date of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$g', 'Miscellaneous information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$h', 'Medium ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$k', 'Form subheading ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$l', 'Language of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$m', 'Medium of performance for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$n', 'Number of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$o', 'Arranged statement for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$p', 'Name of part/section of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$r', 'Key for music ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$s', 'Version ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$t', 'Title of a work ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$v', 'Volume/sequential designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$w', 'Bibliographic record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$x', 'International Standard Serial Number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$0', 'Authority record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('830', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('841', 'HOLDINGS CODED DATA VALUES', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('842', 'TEXTUAL PHYSICAL FORM DESIGNATOR', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('843', 'REPRODUCTION NOTE', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('844', 'NAME OF UNIT', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('845', 'TERMS GOVERNING USE AND REPRODUCTION NOTE', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('850', 'HOLDING INSTITUTION', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('850', '$a', 'Holding institution ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('850', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('852', 'LOCATION', 'Shelving scheme', 'Shelving order');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$a', 'Location ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$b', 'Sublocation or collection ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$c', 'Shelving location ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$d', 'Former shelving location');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$e', 'Address ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$f', 'Coded location qualifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$g', 'Non-coded location qualifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$h', 'Classification part ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$i', 'Item part ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$j', 'Shelving control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$k', 'Call number prefix ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$l', 'Shelving form of title ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$m', 'Call number suffix ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$n', 'Country code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$p', 'Piece designation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$q', 'Piece physical condition ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$s', 'Copyright article-fee code ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$t', 'Copy number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$u', 'Uniform Resource Identifier');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$x', 'Nonpublic note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$z', 'Public note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$2', 'Source of classification or shelving scheme ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('852', '$8', 'Sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('853', 'CAPTIONS AND PATTERN--BASIC BIBLIOGRAPHIC UNIT', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('854', 'CAPTIONS AND PATTERN--SUPPLEMENTARY MATERIAL', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('855', 'CAPTIONS AND PATTERN--INDEXES', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('856', 'ELECTRONIC LOCATION AND ACCESS', 'Access method', 'Relationship');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$a', 'Host name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$b', 'Access number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$c', 'Compression information ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$d', 'Path ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$f', 'Electronic name ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$h', 'Processor of request ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$i', 'Instruction ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$j', 'Bits per second ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$k', 'Password ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$l', 'Logon ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$m', 'Contact for access assistance ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$n', 'Name of location of host ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$o', 'Operating system ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$p', 'Port ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$q', 'Electronic format type ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$r', 'Settings ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$s', 'File size ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$t', 'Terminal emulation ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$u', 'Uniform Resource Identifier ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$v', 'Hours access method available ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$w', 'Record control number ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$x', 'Nonpublic note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$y', 'Link text ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$z', 'Public note ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$2', 'Access method ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$3', 'Materials specified ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$6', 'Linkage ');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('856', '$8', 'Field link and sequence number ');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('863', 'ENUMERATION AND CHRONOLOGY--BASIC BIBLIOGRAPHIC UNIT', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('864', 'ENUMERATION AND CHRONOLOGY--SUPPLEMENTARY MATERIAL', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('865', 'ENUMERATION AND CHRONOLOGY--INDEXES', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('866', 'TEXTUAL HOLDINGS--BASIC BIBLIOGRAPHIC UNIT', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('867', 'TEXTUAL HOLDINGS--SUPPLEMENTARY MATERIAL', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('868', 'TEXTUAL HOLDINGS--INDEXES', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('876', 'ITEM INFORMATION--BASIC BIBLIOGRAPHIC UNIT', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('877', 'ITEM INFORMATION--SUPPLEMENTARY MATERIAL', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('878', 'ITEM INFORMATION--INDEXES', '', '');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('880', 'ALTERNATE GRAPHIC REPRESENTATION', 'Same as associated field', 'Same as associated field');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('880', '$6', 'Linkage');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('882', 'REPLACEMENT RECORD INFORMATION', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('882', '$a', 'Replacement title');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('882', '$i', 'Explanatory text');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('882', '$w', 'Replacement bibliographic record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('882', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('882', '$8', 'Field link and sequence number');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('886', 'FOREIGN MARC INFORMATION FIELD', 'Type of field', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$a', 'Tag of the foreign MARC field');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$b', 'Content of the foreign MARC field');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$c', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$d', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$e', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$f', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$g', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$h', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$i', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$j', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$k', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$l', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$m', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$n', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$o', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$p', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$q', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$r', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$s', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$t', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$u', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$v', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$w', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$x', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$y', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$z', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$0', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$1', 'Foreign MARC subfield');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$2', 'Source of data');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$3', 'Source of data');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$4', 'Source of data');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$5', 'Source of data');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$6', 'Source of data');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$7', 'Source of data');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$8', 'Source of data');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('886', '$9', 'Source of data');

INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) VALUES ('887', 'NON-MARC INFORMATION FIELD', 'Undefined', 'Undefined');


INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('887', '$a', 'Content of non-MARC field');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) VALUES('887', '$2', 'Source of data');


INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('LDR', 'LEADER', '', '');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('012', 'CONSER', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$a', 'Priority byte');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$b', 'Non-permanent distribution code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$c', 'Major/minor change byte');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$d', 'Permanent distribution code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$e', 'Special LC projects');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$f', 'OCAT certification');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$g', 'Type of cataloging code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$h', 'Non-established name indicator');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$i', 'NST publication date code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$j', 'ISSN distribution');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$k', 'ISSN on publication');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$l', 'Communication with publisher');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$m', 'Communication with USP');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('012', '$z', 'Record status override');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('019', 'OCLC Control Number Cross-Reference', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('019', '$a', 'OCLC control number of merged and deleted record');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('029', 'Other System Control Number', 'Type of system control number', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('029', '$a', 'OCLC library identifier');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('029', '$b', 'System control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('029', '$c', 'OAI set name');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('029', '$t', 'Content type identifier');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('049', 'Local Holding', 'Controls printing', 'Indicates the completeness of holdings data');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$a', 'Holding library');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$c', 'Copy statement');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$d', 'Definition of bibliographic subdivisions');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$l', 'Local processing data');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$m', 'Missing elements');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$n', 'Notes about holdings');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$o', 'Local processing data');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$p', 'Secondary bibliographic subdivision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$q', 'Third bibliographic subdivision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$r', 'Fourth bibliographic subdivision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$s', 'Fifth bibliographic subdivision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$t', 'Sixth bibliographic subdivision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$u', 'Seventh bibliographic subdivision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$v', 'Primary bibliographic subdivision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('049', '$y', 'Inclusive dates of publication or coverage');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('069', 'Other System Control Number', 'NLM Unique ID number or a serial record control number', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('069', '$a', 'Control number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('090', 'Locally Assigned LC-type Call Number', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('090', '$a', 'Classification number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('090', '$b', 'Local Cutter number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('090', '$e', 'Feature heading');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('090', '$f', 'Filing suffix');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('092', 'Locally Assigned Dewey Call Number', 'DDC edition', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('092', '$a', 'Classification number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('092', '$b', 'Item number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('092', '$e', 'Feature heading');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('092', '$f', 'Filing suffix');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('092', '$2', 'Edition number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('096', 'Locally Assigned NLM-type Call Number', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('096', '$a', 'Classification number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('096', '$b', 'Item number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('096', '$e', 'Feature heading');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('096', '$f', 'Filing suffix');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('097', 'Locally Assigned LC-type Call Number', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('097', '$a', 'Classification number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('097', '$b', 'Item number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('350', 'Price', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('350', '$a', 'Price (do not use very sloppy)');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('588', 'Source of Description Note', 'Display constant controller', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('588', '$a', 'Source of description note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('588', '$5', 'Institution to which field applies');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('588', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('588', '$8', 'Field link and sequence number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('590', 'Local Note', 'Privacy', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('590', '$a', 'Local note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('590', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('590', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('590', '$8', 'Field link and sequence number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('591', 'Local Note', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('591', '$a', 'Local note');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('592', 'Local Note', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('592', '$a', 'Local note');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('593', 'Local Note', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('593', '$a', 'Local note');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('595', 'Local Note', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('595', '$a', 'Local note');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('598', 'Local Note', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('598', '$a', 'Local note');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('690', 'Local Subject Added Entry--Topical Term', 'Level of subject', 'Thesaurus');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$a', 'Topical term or geographic name as entry element');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$b', 'Topical term following geographic name as entry element');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$c', 'Location of event');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$d', 'Active dates');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$e', 'Relator term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$g', 'Miscellaneous information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$v', 'Form subdivision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$x', 'General subdivision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$y', 'Chronological subdivision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$z', 'Geographic subdivision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$2', 'Source of heading or term');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$8', 'Field link and sequence number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('690', '$9', 'Special entry');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('695', 'Added Class Number', 'Type of edition', 'Classification scheme');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('695', '$a', 'Added class number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('695', '$b', 'Item number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('695', '$e', 'Heading');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('695', '$f', 'Filing suffix');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('695', '$2', 'Edition number');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('906', 'Local Data', 'Defined for local use', 'Defined for local use');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('906', '$a', 'Defined for local use');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('906', '$b', 'Defined for local use');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('906', '$c', 'Defined for local use');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('906', '$d', 'Defined for local use');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('906', '$e', 'Defined for local use');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('906', '$f', 'Defined for local use');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('906', '$g', 'Defined for local use');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('906', '$y', 'Defined for local use');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('910', 'Local Data', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('910', '$a', 'Local data');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('920', 'LOCAL SELECTION DECISION', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('920', '$a', 'Selection decision');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('922', 'LOCAL BOOK SOURCE', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('922', '$a', 'Book source acquisition information');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('923', 'LOCAL SUPPLIER INVOICE OR SHIPMENT ID', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('923', '$a', 'Additional information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('923', '$d', 'Formatted date');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('923', '$n', 'Shipment/invoice number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('923', '$s', 'Supplier');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('925', 'LOCAL SELECTION DECISION', 'Current Decision', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('925', '$a', 'Selection decision for LC');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('925', '$b', 'Number of shelf copies/sets desired');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('925', '$c', 'Acquisition conditions');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('925', '$d', 'Disposition of unwanted material with outside agency');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('925', '$e', 'Comment related to selection decision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('925', '$h', 'Custodial division');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('925', '$x', 'Responsibility for selection decision');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('925', '$y', 'Office copy request');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('925', '$z', 'Reference assignment request');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('936', 'CONSER/OCLC Miscellaneous Data', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('936', '$a', 'CONSER/OCLC miscellaneous data');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('938', 'Vendor-Specific Ordering Data', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('938', '$a', 'Full name of vendor');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('938', '$b', 'OCLC-defined symbol for vendor');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('938', '$c', 'Terms of availability');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('938', '$d', 'Vendor net price');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('938', '$i', 'Inventory number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('938', '$n', 'Vendor control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('938', '$s', 'Vendor status');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('938', '$z', 'Note');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('947', 'Local Data', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('947', '$a', 'Defined for local use');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('948', 'Local Data', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('948', '$a', 'Defined for local use');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('948', '$2', 'Defined for local use');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('952', 'LOCAL CATALOGER’S PERMANENT NOTE', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('952', '$a', 'Cataloger’s note');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('955', 'LOCAL CATALOGER’S PERMANENT NOTE', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$a', 'IBC processing/other forwarding or tracking information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$b', '[Unused subfield]');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$c', 'Descriptive cataloging tracking information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$d', 'Subject cataloging tracking information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$e', 'Shelflisting/end-stage processing tracking information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$f', 'CIP verification tracking information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$g', 'CIP verification end-stage processing tracking information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$h', 'MLC tracking information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$i', 'Whole item cataloging tracking information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$j', 'ISSN pre-publication assignment tracking information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$k', 'ISSN post-publication assignment tracking information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$l', 'Holdings conversion and inventory tracking information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('955', '$m', 'Bibliographic record cancellation tracking information');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('963', 'LOCAL RELATED CIP OR PCN DATA', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('963', '$a', 'Publisher contact name/phone');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('963', '$b', 'Miscellaneous note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('963', '$c', 'Congressional loan legend');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('984', 'LOCAL SHELFLIST COMPARE STATUS', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('984', '$a', 'Comparison file');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('984', '$b', 'Note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('984', '$d', 'Date of comparison (yyyy-mm-dd)');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('985', 'LOCAL RECORD HISTORY', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('985', '$a', 'Agency that keyed record/record history');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('985', '$b', 'Network used for first level keying');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('985', '$c', 'Network transmitting record to LC');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('985', '$d', 'Date record entered in original or transmitting network');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('985', '$e', 'Responsible LC application or project');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('985', '$f', 'Online cataloger maintenance [staff code]');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('985', '$g', 'PREMARC maintenance history');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('985', '$h', 'PREMARC maintenance comment');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('987', 'Local Romanization/Conversion History', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('987', '$a', 'Romanization/conversion identifier');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('987', '$b', 'Agency that converted, created or reviewed romanization/conversion');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('987', '$c', 'Date of conversion or review');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('987', '$d', 'Status code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('987', '$e', 'Version of conversion program used');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('987', '$f', 'Note');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('991', 'Local Romanization/Conversion History', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$a', 'Copy location code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$b', 'Sublocation of collection');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$c', 'Shelving location');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$d', 'Date of location change');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$e', 'Box number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$f', 'Oversize location');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$g', 'Location');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$h', 'Classification part');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$i', 'Item part');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$k', 'Call number prefix');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$l', 'Copy location code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$m', 'Call number suffix');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$o', 'Item type');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$p', 'Piece designation');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$r', 'Item use count');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$t', 'Copy number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$u', 'Volume chronology');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$v', 'Volume enumeration');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$w', 'Source file');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$x', 'Nonpublic note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$y', 'Item record note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('991', '$z', 'Public note');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('992', 'LOCAL LOCATION INFORMATION', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$a', 'Location');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$b', 'Sublocation of collection');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$c', 'Shelving location');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$h', 'Classification part');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$i', 'Item part');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$k', 'Call number prefix');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$m', 'Call number suffix');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$o', 'Item type');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$p', 'Piece designation');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$r', 'Item use count');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$t', 'Copy number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$u', 'Volume chronology');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$v', 'Volume enumeration');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$w', 'Source file');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$x', 'Nonpublic note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$y', 'Item record note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('992', '$z', 'Public note');
INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('994', 'OCLC-MARC Transaction Code', 'Undefined', 'Undefined');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('994', '$a', 'Transaction code');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('994', '$b', 'Institution symbol');





INSERT INTO MarcRecordDataFieldType(fieldNumber, description, indicator1, indicator2) 
VALUES ('859', 'LOCAL ELECTRONIC LOCATION AND ACCESS', 'Access method', 'Relationship');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$a', 'Host name');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$b', 'Access number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$c', 'Compression information');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$d', 'Path');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$f', 'Electronic name');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$h', 'Processor of request');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$i', 'Instruction');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$j', 'Bits per second');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$k', 'Password');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$l', 'Logon');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$m', 'Contact for access assistance');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$n', 'Name of location of host');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$o', 'Operating system');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$p', 'Port');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$q', 'Electronic format type');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$r', 'Settings');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$s', 'File size');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$t', 'Terminal emulation');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$u', 'Uniform Resource Identifier');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$v', 'Hours access method available');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$w', 'Record control number');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$x', 'Nonpublic note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$y', 'Link text');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$z', 'Public note');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$2', 'Access method');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$3', 'Materials specified');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$6', 'Linkage');
INSERT INTO MarcRecordDataSubFieldType(fieldNumber, indicator, description) 
VALUES('859', '$8', 'Field link and sequence number');

