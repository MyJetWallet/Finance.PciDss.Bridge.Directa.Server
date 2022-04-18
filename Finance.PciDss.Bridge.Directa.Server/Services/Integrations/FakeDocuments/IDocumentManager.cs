using System.Threading.Tasks;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments
{
    public interface IDocumentManager
    {
        Task<Document> GetAsync(string traderId, string country2);
    }
}