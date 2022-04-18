using Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments.Generators;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments
{
    public interface IFakeDocumentGeneratorProvider
    {
        BaseFakeDocumentGenerator Get(string country2);
    }
}