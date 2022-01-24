using Newtonsoft.Json;

namespace crm.Models
{
    public class Currencies_Dictionary
    {
        [JsonProperty("currency")]
        public Dictionary<string, string> Currencies { get; set; }
    }
}
