using System.Data.SqlClient;

namespace Rittenhouse.RBD.Core.DataAccess.Entities
{
    public interface IDataEntity
    {
        void Populate(SqlDataReader reader);
    }
}
