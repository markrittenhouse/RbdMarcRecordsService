using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcRecordServiceApp.Core
{
        public class HtmlControlBuilder
        {
            //protected static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            public const string LineBreak = "\r\n";

            public const string SGridHeaderBackground = "background-color: #95A4C0;";
            public const string SCartCellDark = "background-color: #D6DBE2;";
            public const string SCartCellLight = "background-color: #E6E9EE;";
            public const string SGridHeader2Background = "background-color: #BEC8D6;";
            public const string SCartCellBase = "font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-style: normal; line-height: normal; font-weight: normal; font-variant: normal; color: #fff;";
            public const string SBookAuthor = "color: #174186; font-family: Arial, Helvetica, sans-serif; font-size: 11px; font-weight: normal;";
            public const string SBookTitle = "color: #174186; font-family: Arial, Helvetica, sans-serif; font-size: 11px; font-weight: bold; text-decoration: none;";
            public const string SSubBookTitle = "color: #174186; font-family: Arial, Helvetica, sans-serif; font-size: 11px; font-style: italic; font-weight: normal; text-decoration: none;";
            public const string TextStyle14 = "font-family: Arial, Helvetica, sans-serif; font-size: 14px; font-weight: bold; color: #000;";
            public const string TextStyle13 = "font-family: Arial, Helvetica, sans-serif; font-size: 13px; font-weight: bold; color: #000;";
            public const string TextStyle12 = "font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: normal; color: #000;";
            public const string TextStyle12Bold = "font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: bold; color: #000;";
            public const string TextStyle11 = "font-family: Arial, Helvetica, sans-serif; font-size: 11px; font-weight: normal; color: #000;";
            public const string TextStyle11Bold = "font-family: Arial, Helvetica, sans-serif; font-size: 11px; font-weight: bold; color: #000;";
            public const string TextStyle11RedBold = "font-family: Arial, Helvetica, sans-serif; font-size: 11px; font-weight: bold; color: #872237;";
            public const string TextStyle10 = "font-family: Arial, Helvetica, sans-serif; font-size: 10px; font-weight: normal; color: #000;";
            public const string TextStyle10Bold = "font-family: Arial, Helvetica, sans-serif; font-size: 10px; font-weight: bold; color: #000;";
            public const string BorderBlackSolidThin = "border:solid 1px #000;";
            public const string BorderTop = "border-top:solid 1px #000;";
            public const string BorderRight = "border-right:solid 1px #000;";
            public const string BorderLeft = "border-left:solid 1px #000;";
            public const string BorderBottom = "border-bottom:solid 1px #000;";
            public const string AlignCenter = "text-align:center;";
            public const string AlignRight = "text-align:right;";
            public const string AlignLeft = "text-align:left;";
            public const string VerticalAlignTop = "vertical-align:top;";
            public const string FooterCopyright = "color: #666666;font-family: Arial,Helvetica,sans-serif;font-size: 11px;font-style: normal;font-weight: bold; text-align:center;";
            public const string BookNonReturnable = "color: #872237; font-family: Arial, Helvetica, sans-serif; font-size: 10px; font-weight: normal";

            public const string DefaultCellHeightStyle = " height:18px;";

            public static string PaddingLeft(int pixels)
            {
                return string.Format("padding-left:{0}px;", pixels);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="type"></param>
            /// <param name="id"></param>
            /// <param name="value"></param>
            /// <param name="cssClass"></param>
            /// <returns></returns>
            public static string BuildInput(string type, string id, string value, string cssClass)
            {
                return string.Format("<input type=\"{0}\" id=\"{1}\" value=\"{2}\" class=\"{3}\" />", type, id, value, cssClass);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="src"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <returns></returns>
            public static string BuildImageTag(string src, string width, string height)
            {
                return string.Format("<img src=\"{0}\" width=\"{1}\" height=\"{2}\" />", src, width, height);
            }

            ///// <summary>
            ///// 
            ///// </summary>
            ///// <returns></returns>
            //public static string GetSiteBaseUrl()
            //{
            //    if ((HttpContext.Current == null))
            //    {
            //        return ConfigSettings.Properties.SiteSubDirectory.ToLower();
            //    }
            //    string siteSubDirectory = string.Format("/{0}/", ConfigSettings.Properties.SiteSubDirectory).ToLower();
            //    string absoluteUri = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
            //    return string.Format("{0}{1}", absoluteUri.Substring(0, absoluteUri.IndexOf(siteSubDirectory, StringComparison.Ordinal)), siteSubDirectory);
            //}
    }
}
