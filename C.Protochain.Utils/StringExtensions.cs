using System;
using System.Security.Cryptography;
using System.Text;

namespace C.Protochain.Utils
{
    public static class StringExtensions
    {
        public static string ComputeSha256Hash(this string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            var sha256Managed = new SHA256Managed();
            byte[] hashBytes = sha256Managed.ComputeHash(bytes);
            var hash = string.Empty;
            foreach (byte b in hashBytes)
            {
                hash += $"{b:x2}";
            }

            return hash;
        }

        public static string ConstructHashable(long index, string data, string previousHash, long timeStamp, long nonce)
        {
            return string.Concat(index, data, previousHash, timeStamp, nonce);
        }
    }
}
