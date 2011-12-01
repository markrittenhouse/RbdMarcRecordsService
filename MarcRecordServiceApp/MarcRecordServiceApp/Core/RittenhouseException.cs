using System;

namespace MarcRecordServiceApp.Core
{
    public class RittenhouseException : Exception
    {
        public string ErrorCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="errorCode"></param>
        public RittenhouseException(string message, Exception innerException, string errorCode)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        public RittenhouseException(string message, string errorCode) 
            : base(message)
        {
            ErrorCode = errorCode;
        }    
    }
}
