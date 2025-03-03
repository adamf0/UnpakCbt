using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Ujian.Application.Ujian.UpdateUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal static class UpdateUjian
    {
        //[Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("Ujian", async (UpdateUjianRequest request, ISender sender) =>
            {
                if (!SecurityCheck.NotContainInvalidCharacters(request.Id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id mengandung karakter berbahaya"))));
                }
                if (!SecurityCheck.NotContainInvalidCharacters(request.IdJadwalUjian))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdJadwalUjian mengandung karakter berbahaya"))));
                }

                if (!SecurityCheck.isValidGuid(request.Id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id harus Guid format"))));
                }
                if (!SecurityCheck.isValidGuid(request.IdJadwalUjian))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdJadwalUjian harus Guid format"))));
                }

                Result result = await sender.Send(new UpdateUjianCommand(
                    Guid.Parse(request.Id),
                    request.NoReg,
                    Guid.Parse(request.IdJadwalUjian),
                    request.Status
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }

        internal sealed class UpdateUjianRequest
        {
            public string Id { get; set; }
            public string NoReg { get; set; }
            public string IdJadwalUjian { get; set; }
            public string Status { get; set; }
        }
    }
}
