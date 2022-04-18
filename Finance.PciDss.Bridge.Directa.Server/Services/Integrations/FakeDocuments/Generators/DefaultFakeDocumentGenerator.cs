namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments.Generators
{
    public class DefaultFakeDocumentGenerator : BaseFakeDocumentGenerator
    {
        protected override string Type => "ID";
        protected override int MinLength => 10;
        protected override int MaxLength => 12;
        protected override CharTypes CharTypes => CharTypes.Digits;
    }
}