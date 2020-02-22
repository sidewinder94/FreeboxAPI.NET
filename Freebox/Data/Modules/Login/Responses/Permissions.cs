using Newtonsoft.Json;

namespace Freebox.Data.Modules.Login.Responses
{
    public class Permissions
    {
        [JsonProperty("settings")]
        public bool Settings { get; set; }

        [JsonProperty("contacts")]
        public bool Contacts { get; set; }

        [JsonProperty("calls")]
        public bool Calls { get; set; }

        [JsonProperty("explorer")]
        public bool Explorer { get; set; }

        [JsonProperty("downloader")]
        public bool Downloader { get; set; }

        [JsonProperty("parental")]
        public bool Parental { get; set; }

        [JsonProperty("pvr")]
        public bool Pvr { get; set; }
    }
}