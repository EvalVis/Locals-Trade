using System;
using System.Security.Cryptography;

namespace RestAPI.Cryptography
{
    public class HashCalculator
    {

        private byte[] salt = new byte[16];

        public string PassHash(string password)
        {
            new RNGCryptoServiceProvider().GetBytes(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        public bool IsGoodPass(string userHash, string loginPassword)
        {
            bool goodpass = false;
            string savedPasswordHash = userHash;
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(loginPassword, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] == hash[i]) goodpass = true;
                else
                {
                    goodpass = false;
                    break;
                }
            }
            return goodpass;
        }

    }
}
