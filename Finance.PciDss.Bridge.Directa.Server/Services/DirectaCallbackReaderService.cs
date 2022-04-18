using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DotNetCoreDecorators;
using Finance.PciDss.Bridge.Directa.Server.Services.Integrations.Contracts.Requests;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using SimpleTrading.ServiceBus.Contracts;

namespace Finance.PciDss.Bridge.Directa.Server.Services
{
    public class DirectaCallbackSubscriberService : IHostedService
    {
        private readonly ISubscriber<DepositDirectaPciDssCallbackBusContract> _subscriberDirectaPciDssCallback;
        private readonly ILogger _logger;
        private readonly ISettingsModelProvider _settingsModelProvider;

        public DirectaCallbackSubscriberService(ISubscriber<DepositDirectaPciDssCallbackBusContract> subscriber,
            ILogger logger, ISettingsModelProvider settingsModelProvider)
        {
            _subscriberDirectaPciDssCallback = subscriber;
            _logger = logger;
            _settingsModelProvider = settingsModelProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriberDirectaPciDssCallback.Subscribe(async e =>
                {
                    if (e is null)
                    {
                        _logger.Information("{Subscriber} event is null", nameof(_subscriberDirectaPciDssCallback));
                    }
                    else
                    {
                        using var activity = new Activity(nameof(_subscriberDirectaPciDssCallback))
                            .SetParentId(e.Activity).Start();
                        _logger.Information("{Subscriber} got event {@event}", nameof(_subscriberDirectaPciDssCallback),
                            e);
                        await SendCallbackAsync(e);
                    }
                }
            );
        }

        private async Task SendCallbackAsync(DepositDirectaPciDssCallbackBusContract directaPciDssCallbackBusContract)
        {
            try
            {
                var settings = _settingsModelProvider.Get();
                var callbackUrl = settings.DirectaNotifyUrl
                    .SetQueryParam("activity", directaPciDssCallbackBusContract.Activity);
                var request = DirectaCallbackRequest.Create(directaPciDssCallbackBusContract.PsTransactionId);

                _logger.Information("Directa send callback {@callback} to url {url}", request, callbackUrl.ToString());
                var result = await callbackUrl.PostJsonAsync(request);
                _logger.Information("Directa return callback response with status {status}", result.StatusCode);
                if (!result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    await Task.Delay(5000);
                    throw new Exception($"Status {result.StatusCode}. Body {content}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Directa Callback failed with exception {error}", e.Message);
                await Task.Delay(5000);
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
