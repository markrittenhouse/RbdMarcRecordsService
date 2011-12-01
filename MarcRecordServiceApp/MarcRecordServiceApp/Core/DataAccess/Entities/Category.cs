using System.Text;

namespace MarcRecordServiceApp.Core.DataAccess.Entities
{
    public class Category
    {
        private string _name;

        public int Id { get; set; }
        public string Code { get; set; }
        public int ProductCount { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _name = value;
                    return;
                }
                if (value.StartsWith("Nursing: "))
                {
                    _name = value.Substring(9);
                    return;
                }
                if (value.StartsWith("ConsHlth: "))
                {
                    _name = value.Substring(10);
                    return;
                }
                _name = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Category = [")
                .AppendFormat("Id = {0}", Id)
                .AppendFormat(", Name = {0}", Name)
                .AppendFormat(", Code = {0}", Code)
                .AppendFormat(", ProductCount = {0}", ProductCount);
            return sb.ToString();
        }

    }
}


