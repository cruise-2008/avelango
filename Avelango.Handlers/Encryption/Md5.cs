using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Avelango.Handlers.Encryption
{
    public static class Md5
    {
        public static string GetHashString(string inputString) {
            var sb = new StringBuilder();
            foreach (var b in GetHash(inputString)) {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }


        public static string GetHashStringAsSha1(string inputString) {
            var sha1 = SHA1.Create();
            var md5 = MD5.Create();
            var md5Res = md5.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            var md5ResString = md5Res.Select(b => b.ToString("x2")).Aggregate("", (total, cur) => total + cur);
            var sha1Res = sha1.ComputeHash(Encoding.UTF8.GetBytes(md5ResString));
            var sha1ResString = sha1Res.Select(b => b.ToString("x2")).Aggregate("", (total, cur) => total + cur);
            return sha1ResString;
        }


        private static IEnumerable<byte> GetHash(string inputString) {
            HashAlgorithm algorithm = MD5.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }
}
