using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.DeleteJadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Presentation.JadwalUjian
{
    internal class DeleteJadwalUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("JadwalUjian/{id}", async (Guid id, ISender sender) =>
            {
                Result result = await sender.Send(
                    new DeleteJadwalUjianCommand(id)
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.JadwalUjian);
        }
    }
}
