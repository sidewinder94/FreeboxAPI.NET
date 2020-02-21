using Newtonsoft.Json;
using Freebox.Exceptions;

namespace Freebox.Data.Modules
{
    /// <summary>
    /// These member are not in the generic <see cref="ApiResponse{T}"/> class to allow usage in <see cref="FreeboxException"/>
    /// </summary>
    public abstract class ApiResponseBase
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error_code", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode { get; set; }

        [JsonProperty("msg", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
