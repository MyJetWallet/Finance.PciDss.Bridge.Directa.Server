using SimpleTrading.SettingsReader;

namespace Finance.PciDss.Bridge.Directa.Server
{
    [YamlAttributesOnly]
    public class SettingsModel
    {
        [YamlProperty("PciDssBridgeDirecta.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("PciDssBridgeDirecta.AuditLogGrpcServiceUrl")]
        public string AuditLogGrpcServiceUrl { get; set; }

        [YamlProperty("PciDssBridgeDirecta.DirectaKey")]
        public string DirectaKey { get; set; }

        [YamlProperty("PciDssBridgeDirecta.DirectaSignature")]
        public string DirectaSignature { get; set; }

        [YamlProperty("PciDssBridgeDirecta.DirectaApiPciDssUrl")]
        public string DirectaApiPciDssUrl { get; set; }

        [YamlProperty("PciDssBridgeDirecta.DirectaRedirectUrl")]
        public string DirectaRedirectUrl { get; set; }

        [YamlProperty("PciDssBridgeDirecta.DirectaNotifyUrl")]
        public string DirectaNotifyUrl { get; set; }

        [YamlProperty("PciDssBridgeDirecta.DirectaServiceBusUrl")]
        public string DirectaServiceBusUrl { get; set; }

        [YamlProperty("PciDssBridgeDirecta.Brand")]
        public string Brand { get; set; }
    }
}