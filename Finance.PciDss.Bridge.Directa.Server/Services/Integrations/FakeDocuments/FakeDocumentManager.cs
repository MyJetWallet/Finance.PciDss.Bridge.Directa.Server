using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments
{
    public class FakeDocumentManager : IDocumentManager
    {
        private readonly IFakeDocumentGeneratorProvider _documentGeneratorProvider;

        private readonly ConcurrentDictionary<string, Lazy<Document>> _cache =
            new();

        public FakeDocumentManager(IFakeDocumentGeneratorProvider documentGeneratorProvider)
        {
            _documentGeneratorProvider = documentGeneratorProvider;
        }

        public Task<Document> GetAsync(string traderId, string country2)
        {
            return Task.FromResult(_cache
                .GetOrAdd(traderId + country2, s => new Lazy<Document>(ValueFactory(traderId, country2))).Value);
        }

        private Document ValueFactory(string traderId, string country2)
        {
            var generator = _documentGeneratorProvider.Get(country2);
            return generator.Generate();
        }
    }
}
