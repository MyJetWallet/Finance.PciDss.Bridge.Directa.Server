namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments.Generators
{
    public class DigitsFakeDocumentGenerator : BaseFakeDocumentGenerator
    {
        public DigitsFakeDocumentGenerator(string type, int minLength, int maxLength)
        {
            Type = type;
            MinLength = minLength;
            MaxLength = maxLength;
        }

        protected override string Type { get; }
        protected override int MinLength { get; }
        protected override int MaxLength { get; }
        protected override CharTypes CharTypes => CharTypes.Digits;

        public static BaseFakeDocumentGenerator Create(string type, int minLength, int maxLength)
        {
            return new DigitsFakeDocumentGenerator(type, minLength, maxLength);
        }
    }
}