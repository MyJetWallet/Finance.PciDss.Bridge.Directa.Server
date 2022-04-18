using Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments;
using NUnit.Framework;

namespace Finance.PciDss.Bridge.Directa.Server.Tests
{
    public class FakeDocumentGeneratorProviderTests
    {
        [Test]
        [Repeat(1000)]
        [TestCase("AR", "DNI", 11, 11)]
        [TestCase("BR", "CPF", 9, 11)]
        [TestCase("CM", "PASS", 9, 11)]
        [TestCase("CA", "PASS", 8, 12)]
        [TestCase("CN", "ID", 3, 20)]
        [TestCase("CL", "ID", 8, 9)]
        [TestCase("CO", "CC", 6, 10)]
        [TestCase("CI", "ID", 8, 12)]
        [TestCase("DO", "CIE", 11, 11)]
        [TestCase("EC", "CC", 9, 11)]
        [TestCase("GH", "ID", 8, 12)]
        [TestCase("IN", "ID", 8, 12)]
        [TestCase("ID", "NIK", 14, 18)]
        [TestCase("KE", "ID", 7, 12)]
        [TestCase("MY", "ID", 10, 14)]
        [TestCase("MX", "PASS", 8, 18)]
        [TestCase("NG", "ID", 9, 12)]
        [TestCase("PA", "PASS", 8, 11)]
        [TestCase("PE", "PASS", 12, 12)]
        [TestCase("PY", null, 0, 0)]
        [TestCase("PH", "PSN", 9, 13)]
        [TestCase("ZA", "ID", 9, 14)]
        [TestCase("TZ", "ID", 8, 20)]
        [TestCase("TH", "ID", 10, 14)]
        [TestCase("UG", "RIC", 11, 15)]
        [TestCase("UY", "CI", 6, 8)]
        [TestCase("VN", "ID", 9, 13)]
        public void Get_ShouldReturnValueBetweenMinAndMaxLength(string country, string type, int minLength,
            int maxLength)
        {
            var documentGeneratorProvider = new FakeDocumentGeneratorProvider();
            var generator = documentGeneratorProvider.Get(country);
            var document = generator.Generate();

            Assert.LessOrEqual(minLength, document.Value.Length);
            Assert.GreaterOrEqual(maxLength, document.Value.Length);
            Assert.AreEqual(type, document.Type);
        }
    }
}
