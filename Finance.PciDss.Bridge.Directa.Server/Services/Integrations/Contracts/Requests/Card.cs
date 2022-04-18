using Destructurama.Attributed;
using Newtonsoft.Json;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Requests
{
    public class Card
    {
        [LogMasked(ShowFirst = 0, ShowLast = 0, PreserveLength = true)]
        [JsonProperty("cvv")]
        public string Cvv { get; set; }

        [LogMasked(ShowFirst = 6, ShowLast = 4, PreserveLength = true)]
        [JsonProperty("card_number")]
        public string CardNumber { get; set; }

        [LogMasked(ShowFirst = 0, ShowLast = 0, PreserveLength = true)]
        [JsonProperty("expiration_month")]
        public string ExpirationMonth { get; set; }

        [LogMasked(ShowFirst = 0, ShowLast = 0, PreserveLength = true)]
        [JsonProperty("expiration_year")]
        public string ExpirationYear { get; set; }

        [LogMasked(ShowFirst = 1, ShowLast = 1, PreserveLength = true)]
        [JsonProperty("holder_name")]
        public string HolderName { get; set; }
    }
}
