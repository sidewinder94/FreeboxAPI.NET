using System.Collections.Generic;
using Freebox.Converters;
using Newtonsoft.Json;

namespace Freebox.Data.Modules.Lan.Reponses;

public class HostResponse : IFreeboxApiResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("primary_name")]
    public string PrimaryName { get; set; }
    
    [JsonProperty("host_type")]
    [JsonConverter(typeof(EnumConverter))]
    public HostType HostType { get; set; }
    
    [JsonProperty("primary_name_manual")]
    public bool PrimaryNameManual { get; set; }

    [JsonProperty("l2ident")]
    public IReadOnlyList<Layer2IdentResponse> KnownLayer2Addresses { get; set; }
    
    [JsonProperty("vendor_name")]
    public string VendorName { get; set; }
    
    [JsonProperty("persistent")]
    public bool Persistent { get; set; }
    
    [JsonProperty("reachable")]
    public bool Reachable { get; set; }
    
    // last_time_reachable timestamp
    
    [JsonProperty("active")]
    public bool Active { get; set; }
    
    // last_activity timestamp
    
    [JsonProperty("names")]
    public IReadOnlyCollection<HostNameResponse> KnownNames { get; set; }
    
    [JsonProperty("l3connectivities")]
    public IReadOnlyCollection<Layer3ConnectivityResponse> Layer3Connectivity { get; set; }
}