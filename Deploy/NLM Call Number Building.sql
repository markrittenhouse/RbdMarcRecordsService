
drop table MarcRecordDataCallNumber
go
create Table MarcRecordDataCallNumber(
	marcRecordId int not null,
	callNumber varchar(255) null,
	category varchar(500) null,
	providerId int not null,
	dateCreated datetime not null,
	dateUpdated datetime null,
	 CONSTRAINT [UX_MarcRecordDataCallNumber_MarcRecordId_ProviderId] UNIQUE NONCLUSTERED 
	(
		marcRecordId ASC,
		providerId ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 100) ON [PRIMARY]
)

go

insert into MarcRecordDataCallNumber(marcRecordId, providerId, dateCreated, callNumber)
select mrf.marcRecordId, mrf.marcRecordProviderTypeId, GETDATE()
, STUFF(( SELECT ', ' + sub.subFieldValue
				from MarcRecordDataSubField sub 
				WHERE mrf.marcRecordDataFieldId = sub.marcRecordDataFieldId
				FOR XML PATH(''),TYPE)
				.value('.','NVARCHAR(MAX)'),1,2,'') AS value
from MarcRecordDataField mrf
join (
	select min(mrf.marcRecordDataFieldId) marcRecordDataFieldId, mrf.marcRecordId
	from MarcRecordDataField mrf
	where mrf.fieldNumber = '060' and mrf.marcRecordProviderTypeId = 2 and mrf.fieldIndicator like '1%'
	group by mrf.marcRecordId
) mrfSingle on mrf.marcRecordDataFieldId = mrfSingle.marcRecordDataFieldId and mrf.marcRecordId = mrfSingle.marcRecordId
left join MarcRecordDataCallNumber mrcn on mrfSingle.marcRecordId = mrcn.marcRecordId and mrf.marcRecordProviderTypeId = mrcn.providerId
where mrcn.marcRecordId is null


--update MarcRecordDataCallNumber
--set callNumber = x.value, dateUpdated = GETDATE()
--from MarcRecordDataCallNumber mrcn
--join (
--	select mrf.marcRecordId, mrf.marcRecordProviderTypeId, mrf.dateCreated
--	, STUFF(( SELECT ', ' + sub.subFieldValue
--					from MarcRecordDataSubField sub 
--					WHERE mrf.marcRecordDataFieldId = sub.marcRecordDataFieldId
--					FOR XML PATH(''),TYPE)
--					.value('.','NVARCHAR(MAX)'),1,2,'') AS value
--	from MarcRecordDataField mrf
--	join (
--		select min(mrf.marcRecordDataFieldId) marcRecordDataFieldId, mrf.marcRecordId
--		from MarcRecordDataField mrf
--		where mrf.fieldNumber = '060' and mrf.marcRecordProviderTypeId = 2 and mrf.fieldIndicator like '1%'
--		group by mrf.marcRecordId
--	) mrfSingle on mrf.marcRecordDataFieldId = mrfSingle.marcRecordDataFieldId and mrf.marcRecordId = mrfSingle.marcRecordId
--) as x on x.marcRecordId = mrcn.marcRecordId and mrcn.providerId = x.marcRecordProviderTypeId
--where x.dateCreated > isnull(mrcn.dateUpdated, mrcn.dateCreated)


go 
update MarcRecordDataCallNumber
set category = replace(subTest.subFieldValue, '''', ''''), dateUpdated = GETDATE()
from MarcRecordDataCallNumber mrcn
join (
	select mrf.marcRecordId, min(sub.marcRecordDataSubFieldsId) marcRecordDataSubFieldsId, mrf.marcRecordProviderTypeId
	from MarcRecordDataSubField sub
	join MarcRecordDataField mrf on mrf.marcRecordDataFieldId = sub.marcRecordDataFieldId and mrf.marcRecordProviderTypeId = 2
	where mrf.fieldNumber = '650' and sub.subFieldIndicator = '$a' 
	group by mrf.marcRecordId, mrf.marcRecordProviderTypeId
) as x on x.marcRecordId = mrcn.marcRecordId and mrcn.providerId = x.marcRecordProviderTypeId
join MarcRecordDataSubField subTest on x.marcRecordDataSubFieldsId = subTest.marcRecordDataSubFieldsId 
where category is null or category <> substring(replace(subTest.subFieldValue, '''', ''''), 0, 500)