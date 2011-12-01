using System.Data.SqlClient;
using MarcRecordServiceApp.Core.DataAccess.Factories.Base;

namespace MarcRecordServiceApp.Core.DataAccess.SqlCommandParameters
{

    public class ByteArrayParameter : FactoryBase, ISqlCommandParameter
    {
        public string Name { get; set; }
        public byte[] Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ByteArrayParameter()
        {            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ByteArrayParameter(string name, byte[] value)
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
