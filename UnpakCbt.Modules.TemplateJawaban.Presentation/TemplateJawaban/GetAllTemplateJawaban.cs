using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetAllTemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Presentation.TemplateJawaban
{
    internal class GetAllTemplateJawaban
    {
        [Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("TemplateJawaban", async (ISender sender) =>
            {
                Result<List<TemplateJawabanResponse>> result = await sender.Send(new GetAllTemplateJawabanQuery());

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.TemplateJawaban).RequireAuthorization();
        }
    }
}
