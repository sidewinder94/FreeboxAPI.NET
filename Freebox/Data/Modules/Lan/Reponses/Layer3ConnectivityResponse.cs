
using Freebox.Converters;
using Newtonsoft.Json;

namespace Freebox.Data.Modules.Lan.Reponses;

public class Layer3ConnectivityResponse
{
    [JsonProperty("addr")]
    public string Address { get; set; }
    
    [JsonProperty("af")]
    [JsonConverter(typeof(EnumConverter))]
    public AddressFamily AddressFamily { get; set; }
    
    [JsonProperty("active")]
    public bool Active { get; set; }
    
    [JsonProperty("reachable")]
    public bool Reachable { get; set; }
    
    // last_activity timestamp Read-only

    //last_time_reachable timestamp Read-only
}