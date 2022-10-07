using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Freebox.Data.Modules.RRD.Responses;

public class RrdResponse<T> : IFreeboxApiResponse where T : RrdResponseDbIdent
{
    [JsonProperty("date_start")]
    private long dateStart { get; set; }

    [JsonProperty("date_end")]
    private long dateEnd { get; set; }

    [JsonProperty("data")]
    public IReadOnlyCollection<T> Data { get; set; }

    public DateTime DateStart => DateTimeOffset.FromUnixTimeSeconds(this.dateStart).DateTime.ToLocalTime();

    public DateTime DateEnd => DateTimeOffset.FromUnixTimeSeconds(this.dateEnd).DateTime.ToLocalTime();

}