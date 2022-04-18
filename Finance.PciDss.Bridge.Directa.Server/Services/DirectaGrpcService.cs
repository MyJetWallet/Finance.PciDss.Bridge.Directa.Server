using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DotNetCoreDecorators;
using Finance.PciDss.Abstractions;
using Finance.PciDss.Bridge.Directa.Server.Services.Extensions;
using Finance.PciDss.Bridge.Directa.Server.Services.Integrations;
using Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments;
using Finance.PciDss.PciDssBridgeGrpc;
using Finance.PciDss.PciDssBridgeGrpc.Contracts;
using Finance.PciDss.PciDssBridgeGrpc.Contracts.Enums;
using MyCrm.AuditLog.Grpc;
using MyCrm.AuditLog.Grpc.Models;
using Serilog;
using SimpleTrading.Common.Helpers;
using SimpleTrading.GrpcTemplate;
using SimpleTrading.Payments.ServiceBus.Models;
using SimpleTrading.ServiceBus.Contracts;

namespace Finance.PciDss.Bridge.Directa.Server.Services
{
    public class DirectaGrpcService : IFinancePciDssBridgeGrpcService
    {
        private const string PaymentSystemId = "pciDssDirectaBankCards";
        private const string UsdCurrency = "USD";
        private readonly GrpcServiceClient<IMyCrmAuditLogGrpcService> _myCrmAuditLogGrpcService;
        private readonly ISettingsModelProvider _settingsModelProvider;
        private readonly ILogger _logger;
        private readonly IDocumentManager _documentManager;
        private readonly IPublisher<DepositDirectaPciDssCallbackBusContract> _publisher;
        private readonly IDirectaHttpClient _directaHttpClient;

        public DirectaGrpcService(IDirectaHttpClient directaHttpClient,
            GrpcServiceClient<IMyCrmAuditLogGrpcService> myCrmAuditLogGrpcService,
            ISettingsModelProvider settingsModelProvider,
            ILogger logger,
            IDocumentManager documentManager,
            IPublisher<DepositDirectaPciDssCallbackBusContract> publisher)
        {
            _directaHttpClient = directaHttpClient;
            _myCrmAuditLogGrpcService = myCrmAuditLogGrpcService;
            _settingsModelProvider = settingsModelProvider;
            _logger = logger;
            _documentManager = documentManager;
            _publisher = publisher;
        }

        private SettingsModel SettingsModel => _settingsModelProvider.Get();

        public async ValueTask<MakeBridgeDepositGrpcResponse> MakeDepositAsync(MakeBridgeDepositGrpcRequest request)
        {
            _logger.Information("DirectaGrpcService start process MakeBridgeDepositGrpcRequest {@request}", request);
            try
            {
                var country = CountryManager.Iso3ToIso2(request.PciDssInvoiceGrpcModel.Country);
                var document = await _documentManager.GetAsync(request.PciDssInvoiceGrpcModel.TraderId, country);
                var requestToBridge =
                    request.PciDssInvoiceGrpcModel.ToDirectaRestModel(country, SettingsModel, document);
                var response =
                    await _directaHttpClient.RegisterInvoiceAsync(requestToBridge);

                if (response.IsFailed || response.SuccessResult is null || response.SuccessResult.IsFailed())
                {
                    _logger.Information("Directa Fail create invoice. {@response}", response);
                    await SendMessageToAuditLogAsync(request.PciDssInvoiceGrpcModel,
                        "Fail Directa create invoice. Error" +
                        (response.FailedResult ?? response.SuccessResult?.GetError()));
                    return new MakeBridgeDepositGrpcResponse
                    {
                        PsTransactionId = response.SuccessResult?.DepositId,
                        ErrorMessage = response.FailedResult ?? response.SuccessResult?.GetError(),
                        Status = DepositBridgeRequestGrpcStatus.ServerError
                    };
                }

                await SendMessageToAuditLogAsync(request.PciDssInvoiceGrpcModel,
                    $"Created deposit invoice with id {request.PciDssInvoiceGrpcModel.OrderId}");

                await _publisher.PublishAsync(new DepositDirectaPciDssCallbackBusContract
                {
                    Activity = Activity.Current?.Id,
                    CreatedAt = DateTime.UtcNow,
                    OrderId = request.PciDssInvoiceGrpcModel.OrderId,
                    PsTransactionId = response.SuccessResult.DepositId,
                    TraderId = request.PciDssInvoiceGrpcModel.TraderId
                });

                return MakeBridgeDepositGrpcResponse.Create(requestToBridge.SuccessUrl,
                    response.SuccessResult.DepositId, DepositBridgeRequestGrpcStatus.Success);
            }
            catch (Exception e)
            {
                _logger.Error(e, "DirectaGrpcService. MakeDepositAsync failed for traderId {traderId}",
                    request.PciDssInvoiceGrpcModel.TraderId);
                await SendMessageToAuditLogAsync(request.PciDssInvoiceGrpcModel,
                    $"MakeDepositAsync failed for traderId {request.PciDssInvoiceGrpcModel.TraderId}");
                return MakeBridgeDepositGrpcResponse.Failed(DepositBridgeRequestGrpcStatus.ServerError, e.Message);
            }
        }

        public ValueTask<GetPaymentSystemGrpcResponse> GetPaymentSystemNameAsync()
        {
            return new ValueTask<GetPaymentSystemGrpcResponse>(GetPaymentSystemGrpcResponse.Create(PaymentSystemId));
        }

        public ValueTask<GetPaymentSystemCurrencyGrpcResponse> GetPsCurrencyAsync()
        {
            return new ValueTask<GetPaymentSystemCurrencyGrpcResponse>(
                GetPaymentSystemCurrencyGrpcResponse.Create(UsdCurrency));
        }

        public async ValueTask<GetPaymentSystemAmountGrpcResponse> GetPsAmountAsync(GetPaymentSystemAmountGrpcRequest request)
        {
            if (request.Currency.Equals(UsdCurrency, StringComparison.OrdinalIgnoreCase))
            {
                return GetPaymentSystemAmountGrpcResponse.Create(request.Amount, request.Currency);
            }
                
            return default;
        }

        public ValueTask<GetDepositStatusGrpcResponse> GetDepositStatusAsync(GetDepositStatusGrpcRequest request)
        {
            throw new NotImplementedException();
        }

        public ValueTask<DecodeBridgeInfoGrpcResponse> DecodeInfoAsync(DecodeBridgeInfoGrpcRequest request)
        {
            throw new NotImplementedException();
        }

        public ValueTask<MakeConfirmGrpcResponse> MakeDepositConfirmAsync(MakeConfirmGrpcRequest request)
        {
            throw new NotImplementedException();
        }

        private ValueTask SendMessageToAuditLogAsync(IPciDssInvoiceModel invoice, string message)
        {
            return _myCrmAuditLogGrpcService.Value.SaveAsync(new AuditLogEventGrpcModel
            {
                TraderId = invoice.TraderId,
                Action = "deposit",
                ActionId = invoice.OrderId,
                DateTime = DateTime.UtcNow,
                Message = message
            });
        }
    }
}