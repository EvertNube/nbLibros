using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubeBooks.Helpers
{
    public class Functions
    {
        private static Random _random;
        private static string[] _chars;
        private static int _passwordLength = 6;

        //Procedure to override the password generation functionality and return a random password of 6 alphnumberic characters.
        public static string GeneratePassword()
        {
            string _password = String.Empty;
            _random = new Random();
            _chars = InitialiseCharArray();

            for (int i = 1; i < _passwordLength; i++)
            {
                _password += GenerateRandomCharacter();
            }
            return _password;
        }
        //Create a character map of characters we want our passwords to be constructed from.
        private static string[] InitialiseCharArray()
        {
            string[] _s = new string[35];
            //Add numbers, 1 to 9.
            for (int i = 0; i < 10; i++)
            {
                _s[i] = (i + 1).ToString();
            }
            // Add letters, a to z - lowercase only.
            for (int j = 97; j < 123; j++)
            {
                _s[j - 88] = Char.ConvertFromUtf32(j);
            }
            return _s;
        }
        //Procedure to return a random character from our character list.
        private static string GenerateRandomCharacter()
        {
            return _chars[_random.Next(_chars.GetUpperBound(0))];
        }
        public static string GenerateRandomPassword(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";
            char[] chars = new char[length];
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }
    }
}
