using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Reyvart.OAuth2.Odnoklassniki.Helpers
{
    static class CryptoHelper
    {
        public static string GetMd5Hash(string initialStr) {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(initialStr));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
