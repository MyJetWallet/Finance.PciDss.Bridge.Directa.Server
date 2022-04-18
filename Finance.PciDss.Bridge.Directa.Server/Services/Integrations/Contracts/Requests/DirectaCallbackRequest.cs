using Newtonsoft.Json;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Requests
{
    public class DirectaCallbackRequest
    {
        [JsonProperty("deposit_id")] public string PsTransactionId { get; set; }

        public static DirectaCallbackRequest Create(string psTransactionId)
        {
            return new()
            {
                PsTransactionId = psTransactionId
            };
        }
    }
}
