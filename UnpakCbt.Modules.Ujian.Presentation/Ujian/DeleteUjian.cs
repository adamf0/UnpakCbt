using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Ujian.Application.Ujian.DeleteUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal class DeleteUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("Ujian/{id}/{noReg}", async (string id, string noReg, ISender sender) =>
            {
                if (!SecurityCheck.NotContainInvalidCharacters(id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id mengandung karakter berbahaya"))));
                }

                Result result = await sender.Send(
                    new DeleteUjianCommand(Guid.Parse(id), noReg)
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }
    }
}
