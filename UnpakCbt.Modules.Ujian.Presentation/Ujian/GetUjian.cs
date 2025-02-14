using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal static class GetUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("Ujian/{id}", async (Guid id, ISender sender) =>
            {
                Result<UjianResponse> result = await sender.Send(new GetUjianQuery(id));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }
    }
}
