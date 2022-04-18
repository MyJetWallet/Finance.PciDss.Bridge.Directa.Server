using System;
using System.Threading.Tasks;
using Finance.PciDss.Bridge.Directa.Server.Services.Extensions;
using Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Requests;
using Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Responses;
using Flurl;
using Flurl.Http;
using Flurl.Http.Content;
using Newtonsoft.Json;
using Serilog;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations
{
    public class DirectaHttpClient : IDirectaHttpClient
    {
        private readonly ISettingsModelProvider _settingsModelProvider;

        public DirectaHttpClient(ISettingsModelProvider settingsModelProvider)
        {
            _settingsModelProvider = settingsModelProvider;
        }

        private SettingsModel SettingsModel => _settingsModelProvider.Get();

        public async Task<Response<CreateDirectaInvoiceResponse, string>> RegisterInvoiceAsync(
            CreateDirectaInvoiceRequest request)
        {
            var directaApiUrl = SettingsModel.DirectaApiPciDssUrl
                .AppendPathSegments("v3", "deposits");
            var json = JsonConvert.SerializeObject(request);
            var date = DateTime.UtcNow.ToString("s") + "Z";
            var capturedJsonContent = new CapturedJsonContent(json);
            var signature =
                json.BuildDepositKeySignature(SettingsModel.DirectaSignature, date, SettingsModel.DirectaKey);
            Log.Logger.Information(
                "Directa send request : {@requests}, link {link}, date {date}, signature {signature}", request,
                directaApiUrl, date, signature);

            var result = await directaApiUrl
                .AllowHttpStatus("400,401,422")
                .WithHeader("Authorization", signature)
                .WithHeader("X-Login", SettingsModel.DirectaKey)
                .WithHeader("X-Date", date)
                .WithHeader("X-Idempotency-Key", Guid.NewGuid())
                .WithHeader("Content-Type", "application/json")
                .PostAsync(capturedJsonContent);

            return await result.DeserializeTo<CreateDirectaInvoiceResponse, string>();
        }
    }
}