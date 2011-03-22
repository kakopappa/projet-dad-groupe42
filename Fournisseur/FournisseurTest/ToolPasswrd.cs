using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace FournisseurTest
{
   static class ToolPasswrd
    {
        static string HashPassword(string email, string password)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            SHA1 sha1;

            sha1 = new SHA1CryptoServiceProvider();
            originalBytes = Encoding.Unicode.GetBytes(email + "alligator21" + password);
            encodedBytes = sha1.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes).Replace("-", String.Empty);
        }
    }
}
