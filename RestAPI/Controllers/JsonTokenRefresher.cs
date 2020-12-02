using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace RestAPI.Controllers
{
    public class JsonTokenRefresher
    {

        private readonly byte[] key;
        private IHttpContextAccessor context;
        private JsonWebToken jsonWebToken;

        public JsonTokenRefresher(byte[] key, IHttpContextAccessor accessor, JsonWebToken token)
        {
            context = accessor;
            this.key = key;
            jsonWebToken = token;
        }

        public FreshAndRefreshToken Refresh(string currentToken, string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(currentToken,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out validatedToken);
            var token = validatedToken as JwtSecurityToken;
            if (token != null && token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                var claimedId = long.Parse(context.HttpContext.User.Claims
                    .FirstOrDefault(type => type.Value == ClaimTypes.NameIdentifier)?.Value ?? "0");
                return jsonWebToken.Authenticate(claimedId);
            }
            return null;
        }

    }
}
