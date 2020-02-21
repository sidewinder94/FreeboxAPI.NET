using Newtonsoft.Json;

namespace Freebox.Data.Modules.Login.Responses
{
    public class AuthorizeResponse : IFreeboxApiResponse
    {

        [JsonProperty("app_token")]
        public string AppToken { get; set; }

        [JsonProperty("track_id")]
        public int TrackId { get; set; }
    }
}
