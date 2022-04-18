using Newtonsoft.Json;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Requests
{
    public class CreateDirectaInvoiceRequest
    {
        [JsonProperty("amount")] public double Amount { get; set; }
        [JsonProperty("invoice_id")] public string OrderId { get; set; }
        [JsonProperty("country")] public string Country { get; set; }
        [JsonProperty("currency")] public string Currency { get; set; }
        [JsonProperty("success_url")] public string SuccessUrl { get; set; }
        [JsonProperty("error_url")] public string ErrorUrl { get; set; }
        [JsonProperty("back_url")] public string CancelUrl { get; set; }
        [JsonProperty("notification_url")] public string NotifyUrl { get; set; }

        /// <summary>
        ///     Choose if the deposit's fee will be paid by the customer or debited from our balance
        /// </summary>
        [JsonProperty("fee_on_payer")]
        public bool IsFeeOnPayer { get; set; } = true;

        /// <summary>
        ///     Choose if the surcharge will be paid by the customer or debited from our balance
        /// </summary>
        [JsonProperty("surcharge_on_payer")]
        public bool IsSurchargeOnPayer { get; set; } = true;

        [JsonProperty("payer")] public Payer Payer { get; set; }
        [JsonProperty("credit_card")] public Card Card { get; set; }
    }
}