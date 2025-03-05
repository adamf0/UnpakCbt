using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Ujian.Application.Ujian.UpdateCbt;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal static class UpdateUjianCbt
    {
        //[Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("Ujian/Cbt", async (UpdateUjianCbtRequest request, ISender sender) =>
            {
                if (!SecurityCheck.NotContainInvalidCharacters(request.Id))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.NotContainInvalidCharacters(request.IdJadwalUjian))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdJadwalUjian mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.NotContainInvalidCharacters(request.IdJawabanBenar))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdJawabanBenar mengandung karakter berbahaya")));
                }

                if (!SecurityCheck.isValidGuid(request.Id))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id harus Guid format")));
                }
                if (!SecurityCheck.isValidGuid(request.IdJadwalUjian))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdJadwalUjian harus Guid format")));
                }
                if (!SecurityCheck.isValidGuid(request.IdJawabanBenar))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdJawabanBenar harus Guid format")));
                }

                Result result = await sender.Send(new UpdateCbtCommand(
                    Guid.Parse(request.Id),
                    request.NoReg,
                    Guid.Parse(request.IdJadwalUjian),
                    Guid.Parse(request.IdJawabanBenar)
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }

        internal sealed class UpdateUjianCbtRequest
        {
            public string Id { get; set; }
            public string NoReg { get; set; }
            public string IdJadwalUjian { get; set; }
            public string IdJawabanBenar { get; set; }
        }
    }
}
