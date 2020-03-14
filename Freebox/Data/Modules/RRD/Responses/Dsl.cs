using Freebox.Data.Modules.RRD.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Data.Modules.RRD.Responses
{
    public class Dsl : RrdResponseDbIdent
    {
        [JsonIgnore]
        internal override RrdDb Db => RrdDb.Dsl;

        [JsonProperty("rate_up")]
        public long RateUp { get; set; }
        [JsonProperty("rate_down")]
        public long RateDown { get; set; }
        [JsonProperty("snr_up")]
        public long SnrUp { get; set; }
        [JsonProperty("snr_down")]
        public long SnrDown { get; set; }
    }
}
