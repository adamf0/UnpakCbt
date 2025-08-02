using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Ujian.Application.Ujian.CreateUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal static class CreateUjian
    {
        //[Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("Ujian", async (CreateUjianRequest request, ISender sender) =>
            {
                return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Fitur diblock /Ujian/Create")));

                if (!SecurityCheck.NotContainInvalidCharacters(request.IdJadwalUjian))
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdJadwalUjian mengandung karakter berbahaya")));
                }

                Result<Guid> result = await sender.Send(new CreateUjianCommand(
                    Sanitizer.Sanitize(request.NoReg),
                    Guid.Parse(request.IdJadwalUjian)
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);

            }).WithTags(Tags.Ujian);
        }

        internal sealed class CreateUjianRequest
        {            
            public string NoReg { get; set; }
            public string IdJadwalUjian { get; set; }
        }
    }
}
