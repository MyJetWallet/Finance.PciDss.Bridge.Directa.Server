using System;
using Newtonsoft.Json;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Responses
{
    public class CreateDirectaInvoiceResponse : FailedDirectaResponse
    {
        [JsonProperty("deposit_id")] public string DepositId { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("merchant_invoice_id")] public string MerchantInvoiceId { get; set; }
        [JsonProperty("payment_info")] public PaymentInfo PaymentInfo { get; set; }

        public bool IsFailed()
        {
            return !string.IsNullOrEmpty(Code) || PaymentInfo is null ||
                   !PaymentInfo.Result.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsSuccess()
        {
            return !IsFailed();
        }
    }
}
