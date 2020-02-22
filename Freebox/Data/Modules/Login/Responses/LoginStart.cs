using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Data.Modules.Login.Responses
{
    public class LoginStart :IFreeboxApiResponse
    {
        [JsonProperty("logged_in")]
        public bool LoggedIn { get; set; }

        [JsonProperty("challenge")]
        public string Challenge { get; set; }
    }
}
