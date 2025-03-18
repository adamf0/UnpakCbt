using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Ujian.Application.Cbt.GetAllCbt;
using UnpakCbt.Modules.Ujian.Application.Cbt.GetCbt;
using UnpakCbt.Modules.Ujian.Presentation;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal class GetAllCbt
    {
        //[Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("Ujian/Cbt/{uuidUjian}", async (string uuidUjian, ISender sender) =>
            {
                if (!SecurityCheck.NotContainInvalidCharacters(uuidUjian))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "uuidUjian mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.isValidGuid(uuidUjian))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "uuidUjian harus Guid format")));
                }

                Result<List<CbtResponse>> result = await sender.Send(new GetAllCbtByJadwalUjianQuery(
                    Guid.Parse(uuidUjian)
                ));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }
    }
}
