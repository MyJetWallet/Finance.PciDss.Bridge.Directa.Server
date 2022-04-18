using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Finance.PciDss.Bridge.Directa.Server.Services.Extensions
{
    public static class SignatureExtensions
    {
        public const string D24AuthorizationScheme = "D24 ";

        public static string BuildDepositKeySignature(this string json, string apiSignature, string date,
            string depositKey)
        {
            var apiSignatureEncoded = Encoding.UTF8.GetBytes(apiSignature);
            using var hash = new HMACSHA256(apiSignatureEncoded);
            var hmacSha256 = hash.ComputeHash(BuildByteArray(date, depositKey, json));
            return D24AuthorizationScheme + hmacSha256.ToHexString().ToLower();
        }

        private static byte[] BuildByteArray(string date, string apiKey, string jsonPayload)
        {
            using var stream = new MemoryStream();
            var dateEncoded = Encoding.UTF8.GetBytes(date);
            var apiKeyEncoded = Encoding.UTF8.GetBytes(apiKey);
            stream.Write(dateEncoded, 0, dateEncoded.Length);
            stream.Write(apiKeyEncoded, 0, apiKeyEncoded.Length);
            if (!string.IsNullOrWhiteSpace(jsonPayload))
            {
                var jsonPayloadEncoded = Encoding.UTF8.GetBytes(jsonPayload);
                stream.Write(jsonPayloadEncoded, 0, jsonPayloadEncoded.Length);
            }

            return stream.ToArray();
        }

        private static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }
    }
}
