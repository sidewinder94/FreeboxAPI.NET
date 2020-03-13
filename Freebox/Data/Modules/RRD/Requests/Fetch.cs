using Freebox.Converters;
using Newtonsoft.Json;
using System;

namespace Freebox.Data.Modules.RRD.Requests
{
    public class Fetch
    {
        [JsonProperty("db")]
        [JsonConverter(typeof(EnumConverter))]
        public RrdDb Db { get; set; }

        [JsonProperty("date_start", NullValueHandling = NullValueHandling.Ignore)]
        private long? _dateStart => this.DateStart?.Ticks;

        [JsonProperty("date_end", NullValueHandling = NullValueHandling.Ignore)]
        private long? _dateEnd => this.DateEnd?.Ticks;

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(FieldsToStringsArray))]
        public RrdFields? Fields { get; set; }

        public DateTime? DateStart { get; set; }
        
        public DateTime? DateEnd { get; set; }
    }
}
