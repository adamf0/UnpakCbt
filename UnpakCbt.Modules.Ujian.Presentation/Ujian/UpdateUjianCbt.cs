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
                if (!SecurityCheck.NotContainInvalidCharacters(request.UuidUjian))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "UuidUjian mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.NotContainInvalidCharacters(request.UuidTemplateSoal))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "UuidTemplateSoal mengandung karakter berbahaya")));
                }
                if (!SecurityCheck.NotContainInvalidCharacters(request.UuidJawabanBenar))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "UuidJawabanBenar mengandung karakter berbahaya")));
                }

                if (!SecurityCheck.isValidGuid(request.UuidUjian))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "UuidUjian harus Guid format")));
                }
                if (!SecurityCheck.isValidGuid(request.UuidTemplateSoal))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "UuidTemplateSoal harus Guid format")));
                }
                if (!SecurityCheck.isValidGuid(request.UuidJawabanBenar))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "UuidJawabanBenar harus Guid format")));
                }

                Result result = await sender.Send(new UpdateCbtCommand(
                    Guid.Parse(request.UuidUjian),
                    Sanitizer.Sanitize(request.NoReg),
                    Guid.Parse(request.UuidTemplateSoal),
                    Guid.Parse(request.UuidJawabanBenar)
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }

        internal sealed class UpdateUjianCbtRequest
        {
            public string UuidUjian { get; set; }
            public string NoReg { get; set; }
            public string UuidTemplateSoal { get; set; }
            public string UuidJawabanBenar { get; set; }
        }
    }
}
