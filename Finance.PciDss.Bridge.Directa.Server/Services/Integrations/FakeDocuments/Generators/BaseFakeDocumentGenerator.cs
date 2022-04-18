using System;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments.Generators
{
    public abstract class BaseFakeDocumentGenerator
    {
        protected static Random Random = new();
        protected abstract string Type { get; }
        protected abstract int MinLength { get; }
        protected abstract int MaxLength { get; }
        protected abstract CharTypes CharTypes { get; }

        protected virtual string ValueGenerator()
        {
            var length = Random.Next(MinLength, MaxLength + 1);
            return IdGenerator.GetId(length, CharTypes);
        }

        public virtual Document Generate()
        {
            return new()
            {
                Type = Type,
                Value = ValueGenerator()
            };
        }
    }
}