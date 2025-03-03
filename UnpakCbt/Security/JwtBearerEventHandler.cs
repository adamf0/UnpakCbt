using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace UnpakCbt.Api.Security
{
    public class JwtBearerEventHandler
    {
        private readonly IConfiguration _configuration;

        public JwtBearerEventHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtBearerEvents GetEvents()
        {
            return new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var claims = context.Principal?.Claims;
                    if (claims == null)
                    {
                        context.Fail("Token tidak memiliki claim yang valid");
                        return;
                    }

                    // Ambil claim yang diperlukan
                    string subjectClaim = claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                    string jwtIdClaim = claims.FirstOrDefault(c => c.Type == "jti")?.Value;
                    string issuerClaim = claims.FirstOrDefault(c => c.Type == "iss")?.Value;
                    string audienceClaim = claims.FirstOrDefault(c => c.Type == "aud")?.Value;

                    // Validasi apakah claim lengkap
                    if (string.IsNullOrEmpty(subjectClaim) || string.IsNullOrEmpty(jwtIdClaim) ||
                        string.IsNullOrEmpty(issuerClaim) || string.IsNullOrEmpty(audienceClaim))
                    {
                        context.Fail("Token tidak memiliki claim yang lengkap");
                        return;
                    }

                    // Validasi nilai claim dengan konfigurasi
                    if (issuerClaim != (_configuration["Issuer"] ?? "localhost"))
                        context.Fail("Token claim 'iss' tidak valid");

                    if (audienceClaim != (_configuration["Audience"] ?? "localhost"))
                        context.Fail("Token claim 'aud' tidak valid");

                    if (subjectClaim != (_configuration["Subject"] ?? "localhost"))
                        context.Fail("Token claim 'sub' tidak valid");

                    // Validasi jwtIdClaim format (UUID + level)
                    string[] parts = jwtIdClaim.Split('-');
                    if (parts.Length != 6)
                    {
                        context.Fail("Token claim 'jti' tidak valid (format salah)");
                        return;
                    }

                    // Regex untuk UUID v4
                    Regex GuidV4Regex = new(
                       @"^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
                       RegexOptions.Compiled | RegexOptions.IgnoreCase
                    );

                    // Gabungkan bagian UUID
                    string uuid = $"{parts[0]}-{parts[1]}-{parts[2]}-{parts[3]}-{parts[4]}";
                    string level = parts[5];

                    if (!GuidV4Regex.IsMatch(uuid))
                    {
                        context.Fail("Token ditolak karena UUID tidak valid");
                        return;
                    }

                    if (level != "admin")
                    {
                        context.Fail("Token ditolak karena akses tidak diberikan");
                        return;
                    }
                },

                OnAuthenticationFailed = context =>
                {
                    context.Fail("Token tidak valid");
                    return Task.CompletedTask;
                }
            };
        }
    }
}
