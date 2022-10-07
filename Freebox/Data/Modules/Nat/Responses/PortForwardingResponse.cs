using Freebox.Converters;
using Freebox.Data.Modules.Lan.Reponses;
using Newtonsoft.Json;

namespace Freebox.Data.Modules.Nat.Responses;

public class PortForwardingResponse : IFreeboxApiResponse
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }
    
    [JsonProperty("ip_proto")]
    [JsonConverter(typeof(EnumConverter))]
    public IpProtocol IpProtocol { get; set; }
    
    [JsonProperty("wan_port_start")]
    public int WanPortStart { get; set; }
    
    [JsonProperty("wan_port_end")]
    public int WanPortEnd { get; set; }
    
    [JsonProperty("lan_ip")]
    public string LanIp { get; set; }
    
    [JsonProperty("lan_port")]
    public int LanPort { get; set; }
    
    [JsonProperty("hostname")]
    public string HostName { get; set; }
    
    [JsonProperty("host")]
    public HostResponse Host { get; set; }
    
    [JsonProperty("src_ip")]
    public string SourceIp { get; set; }
    
    [JsonProperty("comment")]
    public string Comment { get; set; }
}