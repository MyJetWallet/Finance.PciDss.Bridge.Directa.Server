using System.Collections.Generic;
using Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments.Generators;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments
{
    public class FakeDocumentGeneratorProvider : IFakeDocumentGeneratorProvider
    {
        private static readonly IDictionary<string, BaseFakeDocumentGenerator> DocumentGenerators =
            new Dictionary<string, BaseFakeDocumentGenerator>
            {
                {"AR", DigitsFakeDocumentGenerator.Create("DNI", 11, 11)},
                {"BR", DigitsFakeDocumentGenerator.Create("CPF", 9, 11)},
                {"CM", DigitsFakeDocumentGenerator.Create("PASS", 9, 11)},
                {"CA", DigitsFakeDocumentGenerator.Create("PASS", 8, 12)},
                {"CN", DigitsFakeDocumentGenerator.Create("ID", 3, 20)},
                {"CL", DigitsFakeDocumentGenerator.Create("ID", 8, 9)},
                {"CO", DigitsFakeDocumentGenerator.Create("CC", 6, 10)},
                {"CI", DigitsFakeDocumentGenerator.Create("ID", 8, 12)},
                {"DO", DigitsFakeDocumentGenerator.Create("CIE", 11, 11)},
                {"EC", DigitsFakeDocumentGenerator.Create("CC", 9, 11)},
                {"GH", DigitsFakeDocumentGenerator.Create("ID", 8, 12)},
                {"IN", DigitsFakeDocumentGenerator.Create("ID", 8, 12)},
                {"ID", DigitsFakeDocumentGenerator.Create("NIK", 14, 18)},
                {"KE", DigitsFakeDocumentGenerator.Create("ID", 7, 12)},
                {"MY", DigitsFakeDocumentGenerator.Create("ID", 10, 14)},
                {"MX", DigitsFakeDocumentGenerator.Create("PASS", 8, 18)},
                {"NG", DigitsFakeDocumentGenerator.Create("ID", 9, 12)},
                {"PA", DigitsFakeDocumentGenerator.Create("PASS", 8, 11)},
                {"PE", DigitsFakeDocumentGenerator.Create("PASS", 12, 12)},
                {"PY", EmptyFakeDocumentGenerator.Empty()},
                {"PH", DigitsFakeDocumentGenerator.Create("PSN", 9, 13)},
                {"ZA", DigitsFakeDocumentGenerator.Create("ID", 9, 14)},
                {"TZ", DigitsFakeDocumentGenerator.Create("ID", 8, 20)},
                {"TH", DigitsFakeDocumentGenerator.Create("ID", 10, 14)},
                {"UG", DigitsFakeDocumentGenerator.Create("RIC", 11, 15)},
                {"UY", DigitsFakeDocumentGenerator.Create("CI", 6, 8)},
                {"VN", DigitsFakeDocumentGenerator.Create("ID", 9, 13)},
                {"", new DefaultFakeDocumentGenerator()}
            };

        public BaseFakeDocumentGenerator Get(string country2)
        {
            if (DocumentGenerators.TryGetValue(country2, out var documentGenerator)) return documentGenerator;
            return new DefaultFakeDocumentGenerator();
        }
    }
}