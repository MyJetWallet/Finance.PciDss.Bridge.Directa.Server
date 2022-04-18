using Newtonsoft.Json;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Responses
{
    public class PaymentInfo
    {
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("result")] public string Result { get; set; }
        [JsonProperty("reason")] public string Reason { get; set; }
        [JsonProperty("reason_code")] public string ReasonCode { get; set; }
        [JsonProperty("payment_method")] public string PaymentMethod { get; set; }
        [JsonProperty("payment_method_name")] public string PaymentMethodName { get; set; }
        [JsonProperty("created_at")] public string CreatedAt { get; set; }
    }
}