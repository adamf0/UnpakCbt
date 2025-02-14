using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Presentation.JadwalUjian
{
    internal static class GetJadwalUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("JadwalUjian/{id}", async (Guid id, ISender sender) =>
            {
                Result<JadwalUjianResponse> result = await sender.Send(new GetJadwalUjianQuery(id));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.JadwalUjian);
        }
    }
}
