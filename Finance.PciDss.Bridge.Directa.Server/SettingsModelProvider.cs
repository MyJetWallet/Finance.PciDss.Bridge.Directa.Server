using SimpleTrading.SettingsReader;

namespace Finance.PciDss.Bridge.Directa.Server
{
    public class SettingsModelProvider : ISettingsModelProvider
    {
        public SettingsModel Get()
        {
            return SettingsReader.ReadSettings<SettingsModel>();
        }
    }

    public interface ISettingsModelProvider
    {
        SettingsModel Get();
    }
}
