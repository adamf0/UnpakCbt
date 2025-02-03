using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetAllJadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Presentation;

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
