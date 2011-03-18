using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Client.Tools
{
    public static class Tools
    {
        public static string HashPassword(string email, string password)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            SHA1 sha1;

            sha1 = new SHA1CryptoServiceProvider();
            originalBytes = Encoding.Unicode.GetBytes(email + "alligator21" + password);
            encodedBytes = sha1.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes).Replace("-", String.Empty);
        }

        public static bool isEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
    }
}
