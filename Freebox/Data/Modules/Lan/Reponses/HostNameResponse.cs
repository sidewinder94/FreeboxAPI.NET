using Freebox.Converters;
using Newtonsoft.Json;

namespace Freebox.Data.Modules.Lan.Reponses;

public class HostNameResponse
{
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("source")]
    [JsonConverter(typeof(EnumConverter))]
    public Layer2AddressType Source { get; set; }
}