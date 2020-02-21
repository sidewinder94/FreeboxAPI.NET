using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Freebox.Data
{
    public class ApiInfo
    {
        [JsonProperty("api_version")]
        private string _apiVersion
        {
            set
            {
                this.ApiVersion = new Version(value);
            }
        }

        [JsonProperty("device_type")]
        public string DeviceType { get; private set; }

        [JsonProperty("api_base_url")]
        public string ApiBase { get; private set; }

        [JsonProperty("uid")]
        public string Uid { get; private set; }

        [JsonProperty("https_available")]
        private string _isHttpsAvailable
        {
            set
            {

                if (int.TryParse(value, out int intRepresentation))
                {
                    this.IsHttpsAvailable = Convert.ToBoolean(intRepresentation);
                    return;
                }

                this.IsHttpsAvailable = Convert.ToBoolean(value, CultureInfo.CurrentCulture);
            }
        }

        [JsonProperty("https_port")]
        private string _httpsPort
        {
            set 
            {
                if (int.TryParse(value, out int result))
                {
                    this.HttpsPort = result;
                    return;
                }

                this.HttpsPort = null;
            }
        }

        [JsonProperty("box_model")]
        public string BoxModel { get; private set; }

        [JsonProperty("box_model_name")]
        public string BoxModelName { get; private set; }

        [JsonProperty("api_domain")]
        public string ApiDomain { get; private set; }


        public Version ApiVersion { get; private set; }
        public bool IsHttpsAvailable { get; private set; }
        public int? HttpsPort { get; private set; }

        public Uri ApiUri => new Uri($"http{(IsHttpsAvailable ? "s" : "")}://{(IsHttpsAvailable ? ApiDomain : "mafreebox.freebox.fr") }:{(IsHttpsAvailable ? HttpsPort.Value.ToString(CultureInfo.InvariantCulture) : "80")}{ApiBase}");
    }
}
