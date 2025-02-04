using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.Ujian.Application.Ujian.DeleteUjian;
using UnpakCbt.Modules.Ujian.Presentation;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal class DeleteUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("Ujian/{id}", async (Guid id, ISender sender) =>
            {
                Result result = await sender.Send(
                    new DeleteUjianCommand(id)
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }
    }
}
