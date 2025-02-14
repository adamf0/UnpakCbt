using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.Ujian.Application.Ujian.DeleteUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal class DeleteUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("Ujian/{id}/{noReg}", async (Guid id, string noReg, ISender sender) =>
            {
                Result result = await sender.Send(
                    new DeleteUjianCommand(id, noReg)
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }
    }
}
