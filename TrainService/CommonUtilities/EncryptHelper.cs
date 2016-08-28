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

        public static string Md5Encrypt2(string sourceString)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.Default.GetBytes(sourceString));
            var encryptPwd = "";
            foreach (byte b in result)
            {
                var c = b.ToString("X");
                if (c.Length == 1)
                {
                    c = "0" + c;
                }
                encryptPwd += c;
            }

            return encryptPwd;
        }

        public static string EncryptToSHA1(string str)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] str1 = Encoding.UTF8.GetBytes(str);
            byte[] output = sha1.ComputeHash(str1);
            sha1.Clear();
            (sha1 as IDisposable).Dispose();
            return BitConverter.ToString(output).Replace("-", ""); ;
        }
    }
}
