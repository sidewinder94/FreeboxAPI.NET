using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Data.Modules.RRD.Responses
{
    public class RrdResponse : IFreeboxApiResponse
    {
        [JsonProperty("date_start")]
        private long dateStart { get; set; }

        [JsonProperty("date_end")]
        private long dateEnd { get; set; }

        [JsonProperty("data")]
        public List<object> Data { get; set; }

    }
}
