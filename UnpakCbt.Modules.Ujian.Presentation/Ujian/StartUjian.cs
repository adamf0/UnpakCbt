using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Ujian.Application.Ujian.StartUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal class StartUjian
    {
        //[Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("Ujian/Start/{id}", async (StartUjianRequest request, ISender sender) =>
            {
                if (!SecurityCheck.NotContainInvalidCharacters(request.Id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id mengandung karakter berbahaya"))));
                }
                if (!SecurityCheck.isValidGuid(request.Id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id harus Guid format"))));
                }

                Result result = await sender.Send(
                    new StartUjianCommand(Guid.Parse(request.Id), request.NoReg)
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }
        internal sealed class StartUjianRequest
        {
            public string Id { get; set; }
            public string NoReg { get; set; }
        }
    }
}
