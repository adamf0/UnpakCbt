using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Presentation.TemplatePertanyaan
{
    internal static class GetTemplatePertanyaan
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("TemplatePertanyaan/{id}", async (Guid id, ISender sender) =>
            {
                Result<TemplatePertanyaanResponse> result = await sender.Send(new GetTemplatePertanyaanQuery(id));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.TemplatePertanyaan);
        }
    }
}
