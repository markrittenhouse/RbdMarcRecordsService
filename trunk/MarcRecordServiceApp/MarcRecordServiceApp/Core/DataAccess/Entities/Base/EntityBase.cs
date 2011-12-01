using log4net;

namespace MarcRecordServiceApp.Core.DataAccess.Entities.Base
{
	public class EntityBase
	{
		protected static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
	}
}
