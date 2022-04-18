using Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Requests;
using Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Responses;
using System.Threading.Tasks;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations
{
    public interface IDirectaHttpClient
    {
        Task<Response<CreateDirectaInvoiceResponse, string>> RegisterInvoiceAsync(
            CreateDirectaInvoiceRequest request);
    }
}