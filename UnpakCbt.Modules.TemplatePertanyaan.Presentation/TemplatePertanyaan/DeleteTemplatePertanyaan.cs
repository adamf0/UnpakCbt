using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.DeleteTemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal class DeleteTemplatePertanyaan
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("TemplatePertanyaan/{id}", async (string id, ISender sender) =>
            {
                if (!SecurityCheck.NotContainInvalidCharacters(id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id mengandung karakter berbahaya"))));
                }

                Result result = await sender.Send(
                    new DeleteTemplatePertanyaanCommand(Guid.Parse(id))
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.TemplatePertanyaan);
        }
    }
}
