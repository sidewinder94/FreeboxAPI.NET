using Freebox.Data.Modules;
using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Freebox.Exceptions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Marquer les types ISerializable avec serializable", Justification = "Some of the objects are not serializable")]
    public class FreeboxException : Exception
    {
        public FreeboxException(ApiResponseBase response, HttpResponseMessage responseMessage)
        {
            Response = response;
            ResponseMessage = responseMessage;
        }

        public override string ToString()
        {
            if(this.Response != null && this.ResponseMessage != null)
            {
                return $"API Response : <{Response.ErrorCode}> {Response.Message} ; Server Response : <{ResponseMessage.StatusCode}> {ResponseMessage.ReasonPhrase}";
            }

            return base.ToString();
        }

        public ApiResponseBase Response { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; }

        public FreeboxException()
        {
        }

        public FreeboxException(string message) : base(message)
        {
        }

        public FreeboxException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FreeboxException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
