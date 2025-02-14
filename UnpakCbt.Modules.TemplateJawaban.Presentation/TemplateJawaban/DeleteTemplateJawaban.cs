using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.DeleteTemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Presentation.TemplateJawaban
{
    internal class DeleteTemplateJawaban
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("TemplateJawaban/{id}", async (Guid id, ISender sender) =>
            {
                Result result = await sender.Send(
                    new DeleteTemplateJawabanCommand(id)
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.TemplateJawaban);
        }
    }
}
