using System;
using System.Threading;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Integrations.FakeDocuments
{
    public static class IdGenerator
    {
        private static readonly string Digits = "0123456789";
        private static readonly string Letters = "ABCDEFGHIJKLMNOPQRSTUV";

        private static readonly string Chars = Digits + Letters;
        private static readonly char[] Encode32Chars = Chars.ToCharArray();
        private static readonly char[] EncodeDigits = Digits.ToCharArray();
        private static readonly char[] EncodeLetters = Letters.ToCharArray();

        private static long _lastId = DateTime.UtcNow.Ticks;

        public static string GetId(int length, CharTypes charTypes)
        {
            return GenerateId(Interlocked.Increment(ref _lastId), length, charTypes);
        }

        private static string GenerateId(long id, int maxLength, CharTypes charTypes)
        {
            return string.Create(maxLength, id, (buffer, value) =>
            {
                char[] encoded;
                int charsLength;
                switch (charTypes)
                {
                    case CharTypes.Letters:
                        encoded = EncodeLetters;
                        charsLength = Letters.Length;
                        break;
                    case CharTypes.Digits:
                        encoded = EncodeDigits;
                        charsLength = Digits.Length;
                        break;
                    case CharTypes.DigitsAndLetters:
                        encoded = Encode32Chars;
                        charsLength = Chars.Length;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(charTypes), charTypes, null);
                }

                for (var i = 0; i < maxLength; i++)
                {
                    var toRight = (maxLength - i - 1) * 5;
                    buffer[i] = encoded[(value >> toRight) & (charsLength - 1)];
                }
                //buffer[12] = encode32Chars[value & 31];
                //buffer[11] = encode32Chars[(value >> 5) & 31];
                //buffer[10] = encode32Chars[(value >> 10) & 31];
                //buffer[9] = encode32Chars[(value >> 15) & 31];
                //buffer[8] = encode32Chars[(value >> 20) & 31];
                //buffer[7] = encode32Chars[(value >> 25) & 31];
                //buffer[6] = encode32Chars[(value >> 30) & 31];
                //buffer[5] = encode32Chars[(value >> 35) & 31];
                //buffer[4] = encode32Chars[(value >> 40) & 31];
                //buffer[3] = encode32Chars[(value >> 45) & 31];
                //buffer[2] = encode32Chars[(value >> 50) & 31];
                //buffer[1] = encode32Chars[(value >> 55) & 31];
                //buffer[0] = encode32Chars[(value >> 60) & 31];
            });
        }
    }
}