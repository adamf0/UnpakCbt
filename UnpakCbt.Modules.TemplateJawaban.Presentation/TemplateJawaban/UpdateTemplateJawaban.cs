using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.UpdateTemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Presentation;

namespace UnpakCbt.Modules.TemplateJawaban.Presentation.TemplateJawaban
{
    internal static class UpdateTemplateJawaban
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("TemplateJawaban", async (UpdateTemplateJawabanRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new UpdateTemplateJawabanCommand(
                    request.Id,
                    request.IdTemplateSoal,
                    request.JawabanText,
                    request.JawabanImg
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.TemplateJawaban);
        }

        internal sealed class UpdateTemplateJawabanRequest
        {
            public Guid Id { get; set; }
            public Guid IdTemplateSoal { get; set; }
            public string? JawabanText { get; set; }
            public string? JawabanImg { get; set; }
        }
    }
}
