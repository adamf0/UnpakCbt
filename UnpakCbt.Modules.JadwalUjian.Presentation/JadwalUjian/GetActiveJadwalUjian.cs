using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetActiveJadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Presentation.JadwalUjian
{
    internal class GetActiveJadwalUjian
    {
        [Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("JadwalUjian/active", async (ISender sender, HttpContext context, TokenValidator tokenValidator) =>
            {
                var (isValid, error) = tokenValidator.ValidateToken(context);
                if (!isValid)
                {
                    return error;
                }

                Result<List<JadwalUjianActiveResponse>> result = await sender.Send(new GetActiveJadwalUjianQuery());

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.JadwalUjian).RequireAuthorization();
        }
    }
}
