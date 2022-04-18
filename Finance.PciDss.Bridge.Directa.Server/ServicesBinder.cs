using DotNetCoreDecorators;
using Finance.PciDss.Bridge.Directa.Server.Services.Integrations;
using Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments;
using Microsoft.Extensions.DependencyInjection;
using MyCrm.AuditLog.Grpc;
using MyServiceBus.Abstractions;
using MyServiceBus.TcpClient;
using Serilog;
using SimpleTrading.GrpcTemplate;
using SimpleTrading.MyLogger;
using SimpleTrading.ServiceBus.Contracts;
using SimpleTrading.ServiceBus.PublisherSubscriber.Deposit;
using SimpleTrading.SettingsReader;

namespace Finance.PciDss.Bridge.Directa.Server
{
    public static class ServicesBinder
    {
        public static string AppName { get; private set; } = "Finance.PciDss.BridgeDirecta.Server";

        private static SettingsModel Settings => SettingsReader.ReadSettings<SettingsModel>();

        public static void BindDirectaHttpCLient(this IServiceCollection services)
        {
            services.AddSingleton<IDirectaHttpClient, DirectaHttpClient>();
        }

        public static void BindLogger(this IServiceCollection services, SettingsModel settings)
        {
            var logger = new MyLogger(AppName, settings.SeqServiceUrl);
            services.AddSingleton<ILogger>(logger);
            Log.Logger = logger;
        }

        public static void BindSettings(this IServiceCollection services)
        {
            services.AddSingleton<ISettingsModelProvider, SettingsModelProvider>();
        }

        public static void BindDocumentServices(this IServiceCollection services)
        {
            services.AddSingleton<IDocumentManager, FakeDocumentManager>();
            services.AddSingleton<IFakeDocumentGeneratorProvider, FakeDocumentGeneratorProvider>();
        }

        public static void BindGrpcServices(this IServiceCollection services)
        {
            var clientAuditLogGrpcService = new GrpcServiceClient<IMyCrmAuditLogGrpcService>(
                () => Settings.AuditLogGrpcServiceUrl);

            services.AddSingleton(clientAuditLogGrpcService);
        }

        public static MyServiceBusTcpClient BindServiceBus(this IServiceCollection services)
        {
            var tcpServiceBus = new MyServiceBusTcpClient(
                () => Settings.DirectaServiceBusUrl, AppName);
            var queueName = $"pcidss-bridge-directa-callback-{Settings.Brand.ToLower()}";

            var directaCallbackMyServiceBusSubscriber = new DirectaCallbackMyServiceBusSubscriber(tcpServiceBus,
                Settings.Brand,
                queueName,
                TopicQueueType.PermanentWithSingleConnection, false);
            services.AddSingleton<ISubscriber<DepositDirectaPciDssCallbackBusContract>>(
                directaCallbackMyServiceBusSubscriber);

            var directaCallbackMyServiceBusPublisher =
                new DirectaCallbackMyServiceBusPublisher(tcpServiceBus, Settings.Brand);
            services.AddSingleton<IPublisher<DepositDirectaPciDssCallbackBusContract>>(
                directaCallbackMyServiceBusPublisher);
            return tcpServiceBus;
        }
    }
}
