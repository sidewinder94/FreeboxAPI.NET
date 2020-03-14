using Freebox.Data.Modules.RRD.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Data.Modules.RRD.Responses
{
    public class Net : RrdResponseDbIdent
    {
        [JsonProperty("bw_up")]
        public long BwUp { get; set; }
        [JsonProperty("bw_down")]
        public long BwDown { get; set; }
        [JsonProperty("rate_up")]
        public long RateUp { get; set; }
        [JsonProperty("rate_down")]
        public long RateDown { get; set; }
        [JsonProperty("vpn_rate_up")]
        public long VpnRateUp { get; set; }
        [JsonProperty("vpn_rate_down")]
        public long VpnRateDown { get; set; }

        [JsonIgnore]
        internal override RrdDb Db => RrdDb.Net;
    }
}
