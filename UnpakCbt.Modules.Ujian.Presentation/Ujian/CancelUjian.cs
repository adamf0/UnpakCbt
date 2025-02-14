using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.Ujian.Application.Ujian.CancelUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal class CancelUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("Ujian/Cancel/{id}", async (Guid id, ISender sender) =>
            {
                Result result = await sender.Send(
                    new CancelUjianCommand(id)
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }
    }
}
