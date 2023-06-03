using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;

namespace ThreadboxApi.Services
{
    public class JwtService : IScopedService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IServiceProvider services)
        {
            _configuration = services.GetRequiredService<IConfiguration>();
        }

        public string CreateAccessToken(Guid userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[AppSettings.JwtSecurityKey]));
            var lifetime = Convert.ToInt32(_configuration[AppSettings.JwtExpirationTimeS]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration[AppSettings.JwtValidAudience],
                Issuer = _configuration[AppSettings.JwtValidIssuer],
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddSeconds(lifetime),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}