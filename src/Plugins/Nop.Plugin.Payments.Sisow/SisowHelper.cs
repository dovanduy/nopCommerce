using System.Security.Cryptography;
using System.Text;

namespace Nop.Plugin.Payments.Sisow
{
    internal static class SisowHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string GetSHA1(string key)
        {
            var sha = new SHA1Managed();
            var enc = new UTF8Encoding();
            byte[] bytes = sha.ComputeHash(enc.GetBytes(key));

            string sha1 = "";
            for (int j = 0; j < bytes.Length; j++)
                sha1 += bytes[j].ToString("x2");
            return sha1;
        }


    }
}
