using Freebox.Data.Modules.RRD.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Data.Modules.RRD.Responses;

public class Temp : RrdResponseDbIdent
{
    [JsonIgnore]
    internal override RrdDb Db => RrdDb.Temp;

    [JsonProperty("cpum")]
    public int CpuM { get; set; }
    [JsonProperty("cpub")]
    public int CpuB { get; set; }
    [JsonProperty("sw")]
    public int Sw { get; set; }
    [JsonProperty("hdd")]
    public int Hdd { get; set; }
    [JsonProperty("fan_speed")]
    public int FanSpeed { get; set; }
}