using Freebox.Converters;
using Freebox.Data.Modules.RRD.Responses;
using Newtonsoft.Json;
using System;

namespace Freebox.Data.Modules.RRD.Requests
{
    public class Fetch<T> where T : RrdResponseDbIdent, new()
    {
        [JsonProperty("db")]
        [JsonConverter(typeof(EnumConverter))]
        internal RrdDb Db => new T().Db;

        [JsonProperty("date_start", NullValueHandling = NullValueHandling.Ignore)]
        private long? _dateStart => this.DateStart.HasValue ? (long?)new DateTimeOffset(this.DateStart.Value).ToUnixTimeSeconds() : null;

        [JsonProperty("date_end", NullValueHandling = NullValueHandling.Ignore)]
        private long? _dateEnd => this.DateEnd.HasValue ? (long?)new DateTimeOffset(this.DateEnd.Value).ToUnixTimeSeconds() : null;

        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(FieldsToStringsArray))]
        public RrdFields? Fields { get; set; }

        [JsonIgnore]
        public DateTime? DateStart { get; set; }

        [JsonIgnore]
        public DateTime? DateEnd { get; set; }
    }
}
