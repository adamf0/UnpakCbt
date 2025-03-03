using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Ujian.Application.Ujian.CancelUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal class CancelUjian
    {
        //[Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("Ujian/Cancel/{id}", async (string id, ISender sender) =>
            {
                if (!SecurityCheck.NotContainInvalidCharacters(id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id mengandung karakter berbahaya"))));
                }
                if (!SecurityCheck.isValidGuid(id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id harus Guid format"))));
                }

                Result result = await sender.Send(
                    new CancelUjianCommand(Guid.Parse(id))
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }
    }
}
