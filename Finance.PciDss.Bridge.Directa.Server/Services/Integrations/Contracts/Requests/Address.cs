using Newtonsoft.Json;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Requests
{
    public class Address
    {
        [JsonProperty("street")] public string Street { get; set; }
        [JsonProperty("city")] public string City { get; set; }
        [JsonProperty("zip_code")] public string ZipCode { get; set; }
    }
}