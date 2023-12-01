using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MyServiceApi
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class MainConfiguration
    {
        [JsonProperty(Required = Required.Always)]
        public string ListeningUrl { get; set; } = "http://localhost:15144";
    }
}
