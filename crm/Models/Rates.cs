using Newtonsoft.Json;

namespace crm.Models
{
    public class Rates
    {
        [JsonProperty("motd")]
        public Motd Motd { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, double> Rates_Dictionary { get; set; }
    }

    public partial class Motd
    {
        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

}

