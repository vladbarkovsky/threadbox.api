using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Interfaces;

namespace ThreadboxApi.Application.Services
{
    public class JwtService : IScopedService
    {
        private readonly IConfiguration _configuration;
        private readonly IDateTimeService _dateTimeOffsetService;

        public JwtService(IConfiguration configuration, IDateTimeService dateTimeOffsetService)
        {
            _configuration = configuration;
            _dateTimeOffsetService = dateTimeOffsetService;
        }

        public string CreateAccessToken(string userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[AppSettings.Jwt.SecurityKey]));
            var lifetimeSeconds = Convert.ToInt32(_configuration[AppSettings.Jwt.ExpirationTimeSeconds]);
            var now = _dateTimeOffsetService.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration[AppSettings.Jwt.ValidAudience],
                Issuer = _configuration[AppSettings.Jwt.ValidIssuer],
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }),
                Expires = now.AddSeconds(lifetimeSeconds),
                NotBefore = now,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}