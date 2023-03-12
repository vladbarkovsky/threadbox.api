using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ThreadboxApi.Configuration;

namespace ThreadboxApi.Configuraton.Startup
{
	public class AuthenticationStartup
	{
		public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.SaveToken = false;

					var securityKey = Encoding.UTF8.GetBytes(configuration[AppSettings.JwtSecurityKey]);

					options.TokenValidationParameters = new TokenValidationParameters
					{
						IssuerSigningKey = new SymmetricSecurityKey(securityKey),
						ValidateIssuerSigningKey = true,

						ValidIssuer = configuration[AppSettings.JwtValidIssuer],
						ValidateIssuer = true,

						ValidAudience = configuration[AppSettings.JwtValidAudience],
						ValidateAudience = true,

						RequireExpirationTime = true,
						ClockSkew = TimeSpan.Zero,
						ValidateLifetime = true,
					};
				});
		}

		public static void Configure(IApplicationBuilder app)
		{
			app.UseAuthentication();
		}
	}
}