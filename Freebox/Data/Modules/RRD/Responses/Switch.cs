using Freebox.Data.Modules.RRD.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Data.Modules.RRD.Responses;

public class Switch : RrdResponseDbIdent
{
    [JsonIgnore]
    internal override RrdDb Db => RrdDb.Switch;

    [JsonProperty("rx_1")]
    public long Rx1 { get; set; }
    [JsonProperty("tx_1")]
    public long Rx2 { get; set; }
    [JsonProperty("rx_2")]
    public long Rx3 { get; set; }
    [JsonProperty("tx_2")]
    public long Rx4 { get; set; }
    [JsonProperty("rx_3")]
    public long Tx1 { get; set; }
    [JsonProperty("tx_3")]
    public long Tx2 { get; set; }
    [JsonProperty("rx_4")]
    public long Tx3 { get; set; }
    [JsonProperty("tx_4")]
    public long Tx4 { get; set; }
}