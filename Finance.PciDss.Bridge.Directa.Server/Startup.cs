using System;
using Finance.PciDss.Bridge.Directa.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyServiceBus.TcpClient;
using Prometheus;
using ProtoBuf.Grpc.Server;
using SimpleTrading.BaseMetrics;
using SimpleTrading.ServiceStatusReporterConnector;
using SimpleTrading.SettingsReader;

namespace Finance.PciDss.Bridge.Directa.Server
{
    public class Startup
    {
        private MyServiceBusTcpClient _bus;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            SettingsModel settingsModel = SettingsReader.ReadSettings<SettingsModel>();
            services.BindSettings();
            services.BindLogger(settingsModel);
            services.BindDirectaHttpCLient();
            services.BindGrpcServices();
            services.BindDocumentServices();
            _bus = services.BindServiceBus();
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddControllers();
            services.AddGrpc();
            services.AddCodeFirstGrpc(option =>
            {
                option.Interceptors.Add<ErrorLoggerInterceptor>();
                option.BindMetricsInterceptors();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.BindServicesTree(typeof(Startup).Assembly);
            app.BindIsAlive();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<DirectaGrpcService>();
                endpoints.MapMetrics();
            });

            _bus.Start();
        }
    }
}
