using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace UnpakCbt.Common.Presentation.Security
{
    public class TokenValidator
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenValidator> _logger;

        public TokenValidator(IConfiguration configuration, ILogger<TokenValidator> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public (bool IsValid, IResult? error) ValidateToken(HttpContext context)
        {
            var token = ExtractToken(context);
            if (string.IsNullOrEmpty(token))
            {
                return (false, Results.Problem("Token Is Missing", statusCode: StatusCodes.Status401Unauthorized));
            }

            try
            {
                var principal = ValidateJwtToken(token, out JwtSecurityToken jwtToken);
                if (jwtToken == null)
                {
                    return (false, Results.Problem("Invalid Token", statusCode: StatusCodes.Status401Unauthorized));
                }

                return ValidateTokenClaims(jwtToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token validation failed");
                return (false, Results.Problem("Invalid Token", statusCode: StatusCodes.Status401Unauthorized));
            }
        }

        private string? ExtractToken(HttpContext context)
        {
            return context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        }

        private ClaimsPrincipal ValidateJwtToken(string token, out JwtSecurityToken? jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Key_Signed") ?? _configuration["Jwt:Secret"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Environment.GetEnvironmentVariable("Issuer") ?? _configuration["Jwt:Issuer"],
                ValidAudience = Environment.GetEnvironmentVariable("Audience") ?? _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            var principal = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            jwtToken = validatedToken as JwtSecurityToken;
            return principal;
        }

        private (bool IsValid, IResult? error) ValidateTokenClaims(JwtSecurityToken jwtToken)
        {
            if (jwtToken.Header.Alg != SecurityAlgorithms.HmacSha256)
            {
                return (false, Results.Problem("Invalid algorithm", statusCode: StatusCodes.Status417ExpectationFailed));
            }

            var sub = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (string.IsNullOrEmpty(sub) || sub != (Environment.GetEnvironmentVariable("Sub") ?? _configuration["Jwt:Sub"]))
            {
                return (false, Results.Problem("Invalid sub", statusCode: StatusCodes.Status417ExpectationFailed));
            }

            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;
            if (!ValidateJti(jti))
            {
                return (false, Results.Problem("Invalid JTI", statusCode: StatusCodes.Status417ExpectationFailed));
            }

            return (true, null);
        }

        private bool ValidateJti(string? jti)
        {
            if (string.IsNullOrEmpty(jti)) return false;

            string[] parts = jti.Split('-');
            if (parts.Length != 6) return false;

            Regex guidV4Regex = new(
                @"^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase
            );

            string uuid = $"{parts[0]}-{parts[1]}-{parts[2]}-{parts[3]}-{parts[4]}";
            string level = parts[5] switch
            {
                "a1f8" => "admin",
                _ => null
            };

            return guidV4Regex.IsMatch(uuid) && level == "admin";
        }
    }
}
