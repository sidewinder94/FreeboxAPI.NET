using Freebox.Converters;
using Newtonsoft.Json;

namespace Freebox.Data.Modules.Lan.Reponses;

public class Layer2IdentResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("type")]
    [JsonConverter(typeof(EnumConverter))]
    public Layer2AddressType AddressType { get; set; }
}