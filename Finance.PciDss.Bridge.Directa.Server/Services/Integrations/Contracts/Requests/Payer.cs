using Destructurama.Attributed;
using Newtonsoft.Json;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Requests
{
    public class Payer
    {
        [JsonProperty("id")] public string TraderId { get; set; }
        [LogMasked(ShowFirst = 3, ShowLast = 3, PreserveLength = true)]
        [JsonProperty("email")] public string Email { get; set; }
        [LogMasked(ShowFirst = 1, ShowLast = 1, PreserveLength = true)]
        [JsonProperty("first_name")] public string FirstName { get; set; }
        [LogMasked(ShowFirst = 1, ShowLast = 1, PreserveLength = true)]
        [JsonProperty("last_name")] public string LastName { get; set; }
        [LogMasked(ShowFirst = 3, ShowLast = 1, PreserveLength = true)]
        [JsonProperty("phone")] public string Phone { get; set; }
        [JsonProperty("birth_date")] public string BirthDate { get; set; }
        [JsonProperty("address")] public Address Address { get; set; }
        [JsonProperty("document")] public string Document { get; set; }
        [JsonProperty("document_type")] public string DocumentType { get; set; }
    }
}