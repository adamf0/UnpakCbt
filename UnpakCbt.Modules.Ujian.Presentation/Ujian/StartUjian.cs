using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.Ujian.Application.Ujian.StartUjian;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal class StartUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("Ujian/Start/{id}", async (StartUjianRequest request, ISender sender) =>
            {
                Result result = await sender.Send(
                    new StartUjianCommand(request.Id, request.NoReg)
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }
        internal sealed class StartUjianRequest
        {
            public Guid Id { get; set; }
            public string NoReg { get; set; }
        }
    }
}
