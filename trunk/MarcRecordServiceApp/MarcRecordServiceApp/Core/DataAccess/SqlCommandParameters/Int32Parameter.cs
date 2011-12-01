using System.Data.SqlClient;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;

namespace MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters
{

    public class Int32Parameter : FactoryBase, ISqlCommandParameter
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public Int32Parameter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Int32Parameter(string name, int value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public void SetCommandParmater(SqlCommand command)
        {
            SetCommandParmater(command, Name, Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Name = {0}, Value = {1}]", Name, Value);
        }
    }
}
