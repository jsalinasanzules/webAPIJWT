using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace webAPI.Helpers
{
    public class JwtService
    {
        private string securekey = "secure key for applications Neil";
        public string Generate(int id)
        {
            var ssk = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securekey));
            var credentials = new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            var payload = new JwtPayload(id.ToString(), null, null,null,DateTime.Today.AddDays(1));
            var securityToken = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);

        }

        public JwtSecurityToken verify(string jwt)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(securekey);
            tokenhandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }

    }
}
