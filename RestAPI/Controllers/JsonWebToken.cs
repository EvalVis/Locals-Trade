using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RestAPI.Controllers
{
    public class JsonWebToken
    {

        private string key;
        private JsonRefreshToken jsonRefreshToken;

        public JsonWebToken(string key, JsonRefreshToken refreshToken)
        {
            this.key = key;
            jsonRefreshToken = refreshToken;
        }

        public FreshAndRefreshToken Authenticate(long id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                new Claim[] {new Claim(ClaimTypes.NameIdentifier, id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = jsonRefreshToken.GenerateToken();
            return new FreshAndRefreshToken {FreshToken = tokenHandler.WriteToken(token), RefreshToken = refreshToken};
        }

    }
}
