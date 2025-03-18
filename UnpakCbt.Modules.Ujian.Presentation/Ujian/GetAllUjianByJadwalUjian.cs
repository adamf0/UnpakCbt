using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetAllUjian;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal class GetAllUjianByJadwalUjian
    {
        [Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("Ujian/{uuidJadwalUjian}/List", async (string uuidJadwalUjian, ISender sender, HttpContext context, TokenValidator tokenValidator) =>
            {
                var (isValid, error) = tokenValidator.ValidateToken(context);
                if (!isValid)
                {
                    return error;
                }

                if (!SecurityCheck.NotContainInvalidCharacters(uuidJadwalUjian))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "uuidJadwalUjian mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.isValidGuid(uuidJadwalUjian))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "uuidJadwalUjian harus Guid format")));
                }

                Result<List<UjianDetailResponse>> result = await sender.Send(new GetAllUjianByJadwalUjianQuery(
                    Guid.Parse(uuidJadwalUjian)
                ));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Ujian).RequireAuthorization();
        }
    }
}
