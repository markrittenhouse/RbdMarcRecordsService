using System;
using MarcRecordServiceApp.Core.DataAccess.Entities;
using MarcRecordServiceApp.Core.DataAccess.Entities.Base;

namespace MarcRecordServiceApp.Tasks.MarcRecords
{
	public class MarcFileData2 : EntityBase
    {
        
        public string EncodingLevel { get; private set; }
        private string _mrcFileText;
        public string XmlFileText { get; set; }
        public string MrkFileText { get; set; }
        public string MrcFileText
        {
            get { return _mrcFileText; }
            set
            {
                _mrcFileText = value;
                EncodingLevel = value.Substring(17, 1);
            }
        }

        public Product Product { get; private set; }

        public DateTime ProcessedDate { get; private set; }

        /// <summary>
        /// Need to set product to insert null records when no record is found on external searchs
        /// </summary>
        /// <param name="product"></param>
        public MarcFileData2(Product product)
        {
            Product = product;
            ProcessedDate = DateTime.Now;
        }

        ///// <summary>
        ///// Product Can only be set after the XML is set
        ///// </summary>
        ///// <param name="products"></param>
        //public void SetProduct(Product[] products)
        //{           
        //    try
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.LoadXml(XmlFileText);

        //        XmlNodeList xmlNodeList = doc.GetElementsByTagName("datafield");

        //        List<string> isbns = new List<string>();

        //        foreach (XmlNode node in xmlNodeList.Cast<XmlNode>().Where(node => node.Attributes != null && Convert.ToInt32(node.Attributes["tag"].InnerText) == 20))
        //        {
        //            if (node.HasChildNodes)
        //            {
        //                foreach (XmlNode xmlNode in node.ChildNodes)
        //                {
        //                    string nodeInnterText = xmlNode.InnerText.Replace(".", "");
        //                    int space = nodeInnterText.IndexOf(" ");
        //                    int seperator = nodeInnterText.IndexOf("(");
        //                    if (space > 0 || seperator > 0)
        //                    {
        //                        isbns.Add(space > 0
        //                                  ? nodeInnterText.Substring(0, space)
        //                                  : nodeInnterText.Substring(0, nodeInnterText.IndexOf("(")));
        //                    }
        //                    else
        //                    {
        //                        isbns.Add(nodeInnterText);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                string nodeInnterText = node.InnerText.Replace(".", "");
        //                if (nodeInnterText.Length > 13)
        //                {
        //                    int space = nodeInnterText.IndexOf(" ");
        //                    isbns.Add(space > 0
        //                                  ? nodeInnterText.Substring(0, space)
        //                                  : nodeInnterText.Substring(0, node.InnerText.IndexOf("(")));
        //                }
        //                else
        //                {
        //                    isbns.Add(nodeInnterText);
        //                }
        //            }
        //        }

        //        foreach (KeyValuePair<int, string> indexAndIsbn in from isbn in isbns from indexAndIsbn in IndexAndIsbns where indexAndIsbn.Value.Contains(isbn) select indexAndIsbn)
        //        {
        //            Product = products[indexAndIsbn.Key];
        //            break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message, ex);
        //        throw;
        //    }
        //}


    }
}
