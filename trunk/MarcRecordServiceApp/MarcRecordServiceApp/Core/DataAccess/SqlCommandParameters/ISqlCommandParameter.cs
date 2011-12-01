using System.Data.SqlClient;

namespace MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters
{
    public interface ISqlCommandParameter
    {
        string Name { get; set; }
        void SetCommandParmater(SqlCommand command);
    }
}
