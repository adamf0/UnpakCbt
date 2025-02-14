using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetAllJadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Presentation.JadwalUjian
{
    internal class GetAllJadwalUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("JadwalUjian", async (ISender sender) =>
            {
                Result<List<JadwalUjianResponse>> result = await sender.Send(new GetAllJadwalUjianQuery());

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.JadwalUjian);
        }
    }
}
