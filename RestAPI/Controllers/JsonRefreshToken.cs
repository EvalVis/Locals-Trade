using System;
using System.Security.Cryptography;

namespace RestAPI.Controllers
{
    public class JsonRefreshToken
    {

        public string GenerateToken()
        {
            var bytes = new byte[32];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

    }
}
