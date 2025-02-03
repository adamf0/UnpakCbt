using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.CreateTemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Presentation;

namespace UnpakCbt.Modules.TemplateJawaban.Presentation.TemplateJawaban
{
    internal static class CreateTemplateJawaban
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("TemplateJawaban", async (CreateTemplateJawabanRequest request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(new CreateTemplateJawabanCommand(
                    request.IdTemplateSoal,
                    request.JawabanText,
                    request.JawabanImg
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.TemplateJawaban);
        }

        internal sealed class CreateTemplateJawabanRequest
        {
            public Guid IdTemplateSoal { get; set; }

            public string? JawabanText { get; set; }
            public string? JawabanImg { get; set; }
        }
    }
}
