using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.DeleteTemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal class DeleteTemplatePertanyaan
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("TemplatePertanyaan/{id}", async (Guid id, ISender sender) =>
            {
                Result result = await sender.Send(
                    new DeleteTemplatePertanyaanCommand(id)
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.TemplatePertanyaan);
        }
    }
}
