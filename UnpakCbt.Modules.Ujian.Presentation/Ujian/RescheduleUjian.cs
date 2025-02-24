using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Ujian.Application.Ujian.RescheduleUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal static class RescheduleUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("Ujian/Reschedule", async (RescheduleUjianRequest request, ISender sender) =>
            {
                if (!SecurityCheck.NotContainInvalidCharacters(request.PrevIdJadwalUjian))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "PrevIdJadwalUjian mengandung karakter berbahaya"))));
                }
                if (!SecurityCheck.NotContainInvalidCharacters(request.NewIdJadwalUjian))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "NewIdJadwalUjian mengandung karakter berbahaya"))));
                }

                if (!SecurityCheck.isValidGuid(request.PrevIdJadwalUjian))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "PrevIdJadwalUjian harus Guid format"))));
                }
                if (!SecurityCheck.isValidGuid(request.NewIdJadwalUjian))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "NewIdJadwalUjian harus Guid format"))));
                }

                Result<Guid> result = await sender.Send(new RescheduleUjianCommand(
                    request.NoReg,
                    Guid.Parse(request.PrevIdJadwalUjian),
                    Guid.Parse(request.NewIdJadwalUjian)
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);

            }).WithTags(Tags.Ujian);
        }

        internal sealed class RescheduleUjianRequest
        {            
            public string NoReg { get; set; }
            public string PrevIdJadwalUjian { get; set; }
            public string NewIdJadwalUjian { get; set; }
        }
    }
}
