using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.DeleteTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Presentation;

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
                // hapus file [PR]

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.TemplatePertanyaan);
        }
    }
}
