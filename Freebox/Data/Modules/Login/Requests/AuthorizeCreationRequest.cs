using Newtonsoft.Json;

namespace Freebox.Data.Modules.Login.Requests;

public class AuthorizeCreationRequest
{
    [JsonProperty("app_id")]
    public string AppId { get; internal set; }

    [JsonProperty("app_name")]
    public string AppName { get; set; }

    [JsonProperty("app_version")]
    public string AppVersion { get; set; }

    [JsonProperty("device_name")]
    public string DeviceName { get; set; }

}