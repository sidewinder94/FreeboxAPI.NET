using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Data.Modules.Login.Requests
{
    class SessionStart
    {
        [JsonProperty("app_id")]
        public string AppId { get; internal set; }

        [JsonProperty("app_version")]
        public string AppVersion { get; set; }

        [JsonProperty("password")]
        internal string Password { get; set; }
    }
}
