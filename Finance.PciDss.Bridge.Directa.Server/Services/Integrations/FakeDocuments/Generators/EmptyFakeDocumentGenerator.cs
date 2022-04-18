namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments.Generators
{
    public class EmptyFakeDocumentGenerator : BaseFakeDocumentGenerator
    {
        protected override string Type { get; }
        protected override int MinLength { get; }
        protected override int MaxLength { get; }
        protected override CharTypes CharTypes => CharTypes.Digits;

        protected override string ValueGenerator()
        {
            return string.Empty;
        }

        public static BaseFakeDocumentGenerator Empty()
        {
            return new EmptyFakeDocumentGenerator();
        }
    }
}