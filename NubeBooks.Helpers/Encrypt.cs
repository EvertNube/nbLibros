using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Helpers
{
    public class Encrypt
    {
        public static string GetCrypt(string text)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] inputHashedBytes = hash.ComputeHash(inputBytes);
                string result = BitConverter.ToString(inputHashedBytes).Replace("-", string.Empty).ToLower();
                return result;
            }
        }

        public static bool comparetoCrypt(string text, string crypted) 
        {
            string textCrypted = GetCrypt(text);
            if (textCrypted.Equals(crypted))
                return true;
            else return false;
        }
    }
}
