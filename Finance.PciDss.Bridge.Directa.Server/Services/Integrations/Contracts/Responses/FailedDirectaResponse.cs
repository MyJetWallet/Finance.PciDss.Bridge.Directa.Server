using System;
using Newtonsoft.Json;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Responses
{
    public class FailedDirectaResponse
    {
        [JsonProperty("code")] public string Code { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("details")] public string[] Details { get; set; }
        [JsonProperty("type")] public string Type { get; set; }

        public string GetError()
        {
            return
                $"Message {Description}, Type {Type}, Code {Code}, details {string.Join(',', Details ?? Array.Empty<string>())}";
        }
    }
}