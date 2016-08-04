using System;
using System.Security.Cryptography;
using System.Text;


namespace CommonUtilities
{
    public static class EncryptHelper
    {
        ///   <summary>
        ///   Encrypt a string with MD5
        ///   </summary>
        ///   <param name="sourceString"></param>
        ///   <returns>Encrypt result</returns>
        [Activity]
        public static string Md5Encrypt(string sourceString)
        {
            var result = Encoding.Default.GetBytes(sourceString);
            MD5 md5 = new MD5CryptoServiceProvider();
            var output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");
        }
    }
}
