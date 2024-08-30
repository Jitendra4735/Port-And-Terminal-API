using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Core.Business.Interface;
using WebApi.Utilities.Models;

namespace WebApi.Core.Business
{
    public class AuthenticationProcessor : IAuthenticationProcessor
    {
        private readonly ILogger<AuthenticationProcessor> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticationProcessor(IConfiguration configuration, ILogger<AuthenticationProcessor> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JWT token for the given user account.
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public async Task<string> GenerateJwtToken(UserAccount userAccount)
        {
            _logger.LogInformation($"GenerateJwtToken method to Generates a JWT token for the given user account **STARTS** with UserAccount = {userAccount.ToString()}");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, userAccount.Username),
                    new Claim(ClaimTypes.Email, userAccount.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            // Create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            _logger.LogInformation($"GenerateJwtToken method to Generates a JWT token for the given user account **END**");
            return tokenHandler.WriteToken(token);
        }
    }
}
