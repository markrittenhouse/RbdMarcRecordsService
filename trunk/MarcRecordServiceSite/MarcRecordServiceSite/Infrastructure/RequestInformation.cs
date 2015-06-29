using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MarcRecordServiceSite.Infrastructure
{
    //public class RequestInformation// : IRequestInformation
    //{
    //    public HttpRequestBase Request
    //    {
    //        get { return new HttpRequestWrapper(HttpContext.Current.Request); }
    //    }

    //    public HttpContextBase Context
    //    {
    //        get { return new HttpContextWrapper(HttpContext.Current); }
    //    }

    //    #region IRequestInformation Members

    //    public string Summary()
    //    {
    //        return "Request Details >> Id: {0}, Client Address: {1}, Referring Url: {2}, Session Id: {3}; User Id: {4}".Args(
    //            Id,
    //            ClientAddress,
    //            ReferringUrl,
    //            SessionId,
    //            (Context.User != null && Context.User.Identity != null) ? Context.User.Identity.ToString() : "Not Authenticated"
    //        );
    //    }

    //    public string Details()
    //    {
    //        var builder = new StringBuilder(Summary());
    //        builder.AppendLine();
    //        builder.AppendFormatedLine("\tUrl: {0}".Args(Request.Url));
    //        builder.AppendFormatedLine("\tRawUrl: {0}".Args(Request.RawUrl));
    //        builder.AppendFormatedLine("\tUserAgent: {0}".Args(Request.UserAgent));
    //        builder.AppendFormatedLine("\tContentType: {0}".Args(Request.ContentType));
    //        builder.AppendFormatedLine("\tContentEncoding: {0}".Args(Request.ContentEncoding));
    //        builder.AppendFormatedLine("\tParmas: ");
    //        foreach (var key in Request.Params.Keys)
    //        {
    //            builder.AppendFormatedLine("\t\t{0}:{1}", key, Request.Params[key.ToString()]);
    //        }

    //        return builder.ToString();
    //    }

    //    public string Id { get { return Context.RequestId().ToString(); } }

    //    //public string ClientAddress { get { return Request.UserHostAddress; } }
    //    public string ClientAddress { get { return GetIpAddress(Request); } }

    //    public string ReferringUrl { get { return Request.UrlReferrer == null ? string.Empty : Request.UrlReferrer.ToString(); } }

    //    public string SessionId { get { return Context.Session == null ? string.Empty : Context.Session.SessionID; } }

    //    public string Host { get { return Request.Url != null ? Request.Url.Host : ""; } }

    //    public string Url { get { return string.Format("{0} {1} ({2})", Request.RequestType, Request.RawUrl, Request.ContentType); } }

    //    #endregion

    //    public override string ToString()
    //    {
    //        return "Request Id: {0}".Args(Id);
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="request"></param>
    //    /// <returns></returns>
    //    private static string GetIpAddress(HttpRequestBase request)
    //    {
    //        return string.IsNullOrEmpty(request.UserHostAddress) ? IPAddress.Loopback.GetIPv4Address() : IPAddress.Parse(request.UserHostAddress).GetIPv4Address();
    //    }

    //    public static void AppendFormatedLine(this StringBuilder builder, string message, params object[] args)
    //    {
    //        builder.AppendFormat(message, args);
    //        builder.AppendLine();
    //    }

        
    //}

    public static class IpAddressExtensions
    {
        public static string GetIPv4Address(this IPAddress address)
        {
            string ip4Address = String.Empty;

            foreach (IPAddress ipAddress in Dns.GetHostAddresses(address.ToString()))
            {
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    ip4Address = ipAddress.ToString();
                    break;
                }
            }

            if (ip4Address != String.Empty)
            {
                return ip4Address;
            }

            foreach (IPAddress ipAddress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    ip4Address = ipAddress.ToString();
                    break;
                }
            }

            return ip4Address;
        }

    }
}