

insert into MarcRecordDataCallNumber(marcRecordId, providerId, dateCreated, callNumber)
select x.marcRecordId, x.marcRecordProviderTypeId, GETDATE()
	, STUFF(( SELECT '' + sub.subFieldValue
				from MarcRecordDataSubField sub 
				WHERE x.marcRecordDataFieldId = sub.marcRecordDataFieldId
				FOR XML PATH('')), 1, 0,'') AS value
from (select min(mrf.marcRecordDataFieldId) marcRecordDataFieldId, mrf.marcRecordId, mrf.marcRecordProviderTypeId
		from MarcRecordDataField mrf 
		where mrf.fieldNumber = '050' and mrf.marcRecordProviderTypeId = 1
		group by mrf.marcRecordId, mrf.marcRecordProviderTypeId
	) x
	left join MarcRecordDataCallNumber mrcn on x.marcRecordId = mrcn.marcRecordId and x.marcRecordProviderTypeId = mrcn.providerId
	where mrcn.marcRecordId is null

go

--update MarcRecordDataCallNumber
--set callNumber = xx.value, dateUpdated = GETDATE()
--from MarcRecordDataCallNumber mrcn
--join (
--	select x.marcRecordId, x.marcRecordProviderTypeId, x.dateCreated
--		, STUFF(( SELECT '' + c2.subFieldValue
--			from MarcRecordDataSubField c2 
--			WHERE x.marcRecordDataFieldId = c2.marcRecordDataFieldId
--			FOR XML PATH('')), 1, 0,'') AS value
--	from (select min(xx.marcRecordDataFieldId) marcRecordDataFieldId, xx.marcRecordId, xx.marcRecordProviderTypeId, xx.dateCreated
--			from MarcRecordDataField xx 
--			where xx.fieldNumber = '050' and xx.marcRecordProviderTypeId = 1
--			group by xx.marcRecordId, xx.marcRecordProviderTypeId, xx.dateCreated
--		) x
--	) xx on mrcn.marcRecordId = xx.marcRecordId and mrcn.providerId = xx.marcRecordProviderTypeId
--where xx.dateCreated > isnull(mrcn.dateUpdated, mrcn.dateCreated)


update MarcRecordDataCallNumber
set category = substring(replace(subTest.subFieldValue, '''', ''''), 0, 500), dateUpdated = GETDATE()
from MarcRecordDataCallNumber mrcn
join (
	select p.marcRecordId, min(sub.marcRecordDataSubFieldsId) marcRecordDataSubFieldsId, p.marcRecordProviderTypeId
	from MarcRecordDataSubField sub
	join MarcRecordDataField p on p.marcRecordDataFieldId = sub.marcRecordDataFieldId and p.marcRecordProviderTypeId = 1
	where p.fieldNumber = '650' and sub.subFieldIndicator = '$a' 
	group by p.marcRecordId, p.marcRecordProviderTypeId
) as x on x.marcRecordId = mrcn.marcRecordId and mrcn.providerId = x.marcRecordProviderTypeId
join MarcRecordDataSubField subTest on x.marcRecordDataSubFieldsId = subTest.marcRecordDataSubFieldsId 
where category is null or category <> substring(replace(subTest.subFieldValue, '''', ''''), 0, 500)








--select sum(x.NULL_LC) as 'No LC Number', sum(x.DIFF_LC) as 'Different LC Number', sum(x.SAME_LC) as 'Same LC Number' 
--, sum(x.NULL_NLM) as 'No NLM Number', sum(x.DIFF_NLM) as 'Different NLM Number', sum(x.SAME_NLM) as 'Same NLM Number'
--from (
--select count(p.sku) as 'NULL_LC', 0 as 'DIFF_LC', 0 as 'SAME_LC', 0 as 'NULL_NLM', 0 as 'DIFF_NLM', 0 as 'SAME_NLM'
---- mdt.callNumber, p.lcCallNumber, count(p.sku) as 'Is Null'
--from MarcRecordDataCallNumber mdt
--join MarcRecord mr on mdt.marcRecordId = mr.marcRecordId
--join RittenhouseWeb..Product p on mr.sku = p.sku
--where mdt.providerId = 1 
--and p.lcCallNumber is null
--union all
--select 0, count(p.sku), 0, 0, 0, 0
--from MarcRecordDataCallNumber mdt
--join MarcRecord mr on mdt.marcRecordId = mr.marcRecordId
--join RittenhouseWeb..Product p on mr.sku = p.sku
--where mdt.providerId = 1 
--and p.lcCallNumber <> mdt.callNumber
--union all
--select 0, 0, count(p.sku), 0, 0, 0
--from MarcRecordDataCallNumber mdt
--join MarcRecord mr on mdt.marcRecordId = mr.marcRecordId
--join RittenhouseWeb..Product p on mr.sku = p.sku
--where mdt.providerId = 1 
--and p.lcCallNumber = mdt.callNumber



--union all
--select 0, 0, 0, count(p.sku), 0, 0
--from MarcRecordDataCallNumber mdt
--join MarcRecord mr on mdt.marcRecordId = mr.marcRecordId
--join RittenhouseWeb..Product p on mr.sku = p.sku
--where mdt.providerId = 2
--and p.nlm is null

--union all
--select 0, 0, 0, 0, count(p.sku), 0
--from MarcRecordDataCallNumber mdt
--join MarcRecord mr on mdt.marcRecordId = mr.marcRecordId
--join RittenhouseWeb..Product p on mr.sku = p.sku
--where mdt.providerId = 2
--and p.nlm <> CONCAT('[DNLM: 1.', mdt.category, '.', mdt.callNumber, ']')

--union all
--select 0, 0, 0, 0, 0, count(p.sku)
--from MarcRecordDataCallNumber mdt
--join MarcRecord mr on mdt.marcRecordId = mr.marcRecordId
--join RittenhouseWeb..Product p on mr.sku = p.sku
--where mdt.providerId = 2
--and p.nlm = CONCAT('[DNLM: 1.', mdt.category, '.', mdt.callNumber, ']')
--) x


--[DNLM: 1.Food.QU 145]

--select p.sku, p.nlm, x.nlm
--from RittenhouseWeb..Product p
--join (
--select mr.sku, CONCAT('[DNLM: 1.', mdt.category, '.', mdt.callNumber, ']') nlm
--from MarcRecord mr
--join MarcDataTemp mdt on mr.marcRecordId = mdt.marcRecordId and mdt.providerId = 2
--) x on p.sku = x.sku
--where 
----x.nlm <> p.nlm 
----or 
--p.nlm is null





--select p.nlm,  CONCAT('[DNLM: 1.', mdt.category, '.', mdt.callNumber, ']')
--from MarcRecordDataCallNumber mdt
--join MarcRecord mr on mdt.marcRecordId = mr.marcRecordId
--join RittenhouseWeb..Product p on mr.sku = p.sku
--where mdt.providerId = 2
--and p.nlm <> CONCAT('[DNLM: 1.', mdt.category, '.', mdt.callNumber, ']')



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