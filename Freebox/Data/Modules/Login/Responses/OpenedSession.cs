using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Data.Modules.Login.Responses
{
    public class OpenedSession : IFreeboxApiResponse
    {
        [JsonProperty("session_token")]
        public string SessionToken { get; set; }

        [JsonProperty("challenge")]
        public string Challenge { get; set; }

        [JsonProperty("permissions")]
        public Permissions Permissions { get; set; }

    }
}
