using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freebox.Data.Modules.RRD.Requests;

[Flags]
public enum RrdFields
{
    [JsonProperty("bw_up")]
    NetBwUp = 1,
    [JsonProperty("bw_down")]
    NetBwDown = 2,
    [JsonProperty("rate_up")]
    NetRateUp = 4,
    [JsonProperty("rate_down")]
    NetRateDown = 8,
    [JsonProperty("vpn_rate_up")]
    NetVpnRateUp = 16,
    [JsonProperty("vpn_rate_down")]
    NetVpnRateDown = 32,
    [JsonProperty("cpum")]
    TempCpuM = 64,
    [JsonProperty("cpub")]
    TempCpuB = 128,
    [JsonProperty("sw")]
    TempSw = 256,
    [JsonProperty("hdd")]
    TempHdd = 512,
    [JsonProperty("fan_speed")]
    TempFanSpeed = 1024,
    [JsonProperty("rate_up")]
    DslRateUp = 2048,
    [JsonProperty("rate_down")]
    DslRateDown = 4096,
    [JsonProperty("snr_up")]
    DslSnrUp = 8192,
    [JsonProperty("snr_down")]
    DslSnrDown = 16384,
    [JsonProperty("rx_1")]
    SwitchRx1 = 32768,
    [JsonProperty("tx_1")]
    SwitchRx2 = 65536,
    [JsonProperty("rx_2")]
    SwitchRx3 = 131072,
    [JsonProperty("tx_2")]
    SwitchRx4 = 262144,
    [JsonProperty("rx_3")]
    SwitchTx1 = 524288,
    [JsonProperty("tx_3")]
    SwitchTx2 = 1048576,
    [JsonProperty("rx_4")]
    SwitchTx3 = 2097152,
    [JsonProperty("tx_4")]
    SwitchTx4 = 4194304
}