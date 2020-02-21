using Freebox.Converters;
using Newtonsoft.Json;

namespace Freebox.Data.Modules.Login.Responses
{
    public class AuthorizeProgressResponse : IFreeboxApiResponse
    {
        [JsonProperty("status")]
        [JsonConverter(typeof(EnumConverter))]
        public AuthorizeStatus Status { get; set; }

        [JsonProperty("challenge")]
        public string Challenge { get; set; }

    }
}
