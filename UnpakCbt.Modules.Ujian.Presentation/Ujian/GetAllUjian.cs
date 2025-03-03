using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetAllUjian;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal class GetAllUjian
    {
        //[Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("Ujian", async (ISender sender) =>
            {
                Result<List<UjianResponse>> result = await sender.Send(new GetAllUjianQuery());

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }
    }
}
