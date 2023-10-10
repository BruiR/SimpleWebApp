using Microsoft.IdentityModel.Tokens;
using SimpleWebApp.Domain.Models;
using SimpleWebApp.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleWebApp.Services
{
    public class JWTManagerService : IJWTManagerService
    {
        private readonly IConfiguration _iconfiguration;

        public JWTManagerService(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        public TokenResponse Authenticate(AuthorizedPerson authorizedPerson)
        {
            if (authorizedPerson == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, authorizedPerson.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, authorizedPerson.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new TokenResponse { Token = "Bearer " + tokenHandler.WriteToken(token) };
        }
    }
}
