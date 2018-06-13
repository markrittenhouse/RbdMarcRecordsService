using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;
using MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters;

namespace MarcRecordServiceApp.Core.DataAccess.Factories
{
    public class AdditionalMarcFieldFactory : FactoryBase
    {
        private const string OclcControlNumber = @"
Insert into AdditionalMarcField(marcRecordId, fieldNumber, marcValue, marcRecordProviderTypeId)
select mrf.marcRecordId, mrf.fieldNumber, mrf.marcValue, mrf.marcRecordProviderTypeId
from MarcRecordDataField mrf
where mrf.fieldNumber = '035' and marcValue like '%OCoLC%'
and mrf.marcRecordDataFieldId not in (
	select x.marcRecordDataFieldId --2265
	from MarcRecordDataField x
	left join MarcRecordDataField xx on x.marcRecordId = xx.marcRecordId and xx.fieldNumber = '035' and xx.marcValue like '%OCoLC%' and xx.marcRecordProviderTypeId = 1
	where x.fieldNumber = '035' and x.marcValue like '%OCoLC%'
	and x.marcRecordProviderTypeId = 2
	and x.marcRecordId = mrf.marcRecordId
)
group by mrf.marcRecordId, mrf.fieldNumber, mrf.marcValue, mrf.marcRecordProviderTypeId
order by 1
";

        private const string OclcCrossReferenceControlNumber = @"
Insert into AdditionalMarcField(marcRecordId, fieldNumber, marcValue, marcRecordProviderTypeId)
select mrf.marcRecordId, mrf.fieldNumber, mrf.marcValue, mrf.marcRecordProviderTypeId
from MarcRecordDataField mrf
where mrf.fieldNumber = '019'
and mrf.marcRecordDataFieldId not in (
	select mrf.marcRecordDataFieldId
	from MarcRecordDataField x
	left join MarcRecordDataField xx on x.marcRecordId = xx.marcRecordId  and x.fieldNumber = '019' and xx.marcRecordProviderTypeId = 2
	where 
		x.fieldNumber = '019'
		and x.marcRecordProviderTypeId = 1
		and xx.fieldNumber is not null
		group by x.marcRecordDataFieldId
)
order by 1
";

        private const string LcCallNumber = @"
Insert into AdditionalMarcField(marcRecordId, fieldNumber, marcValue, marcRecordProviderTypeId)
select mrf.marcRecordId, mrf.fieldNumber, mrf.marcValue, mrf.marcRecordProviderTypeId
from MarcRecordDataField mrf
where mrf.fieldNumber = '050'
and mrf.marcRecordDataFieldId not in (
	select mrf.marcRecordDataFieldId
	from MarcRecordDataField mrf
	left join MarcRecordDataField mrf2 on mrf.marcRecordId = mrf2.marcRecordId and mrf2.marcRecordProviderTypeId = 1
	where 
		mrf.fieldNumber = '050'
		and mrf.marcRecordProviderTypeId = 2 and mrf2.fieldNumber is not null
		group by mrf.marcRecordDataFieldId
)
order by 1
";

        private const string NlmCallNumber = @"
Insert into AdditionalMarcField(marcRecordId, fieldNumber, marcValue, marcRecordProviderTypeId)
select mrf.marcRecordId, mrf.fieldNumber, mrf.marcValue, mrf.marcRecordProviderTypeId
from MarcRecordDataField mrf
where mrf.fieldNumber = '060'
and mrf.marcRecordDataFieldId not in (
	select x.marcRecordDataFieldId
	from MarcRecordDataField x
	left join MarcRecordDataField xx on x.marcRecordId = xx.marcRecordId and xx.marcRecordProviderTypeId = 2
	where 
		x.fieldNumber = '060'
		and x.marcRecordProviderTypeId = 1 and xx.fieldNumber is not null
		group by x.marcRecordDataFieldId
)
group by mrf.marcRecordId, mrf.fieldNumber, mrf.marcValue, mrf.marcRecordProviderTypeId
order by 1
";

        public int TruncateAdditionalMarcFields()
        {
            return ExecuteTruncateTable("AdditionalMarcField", Settings.Default.RittenhouseMarcDb);
        }

        public int InsertOclcCrossReferenceControlNumbers()
        {
            return ExecuteInsertStatementReturnRowCount(OclcCrossReferenceControlNumber, new ISqlCommandParameter[0], false, Settings.Default.RittenhouseMarcDb);
        }

        public int InsertOclcControlNumbers()
        {
            return ExecuteInsertStatementReturnRowCount(OclcControlNumber, new ISqlCommandParameter[0], false, Settings.Default.RittenhouseMarcDb);
        }

        public int InsertLcCallNumber()
        {
            return ExecuteInsertStatementReturnRowCount(LcCallNumber, new ISqlCommandParameter[0], false, Settings.Default.RittenhouseMarcDb);
        }

        public int InsertNlmCallNumber()
        {
            return ExecuteInsertStatementReturnRowCount(NlmCallNumber, new ISqlCommandParameter[0], false, Settings.Default.RittenhouseMarcDb);
        }
    }
}
