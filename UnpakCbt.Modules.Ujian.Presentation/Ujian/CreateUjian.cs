using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.Ujian.Application.Ujian.CreateUjian;
using UnpakCbt.Modules.Ujian.Presentation;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal static class CreateUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("Ujian", async (CreateUjianRequest request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(new CreateUjianCommand(
                    request.NoReg,
                    request.IdJadwalUjian
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);

            }).WithTags(Tags.Ujian);
        }

        internal sealed class CreateUjianRequest
        {            
            public string NoReg { get; set; }
            public Guid IdJadwalUjian { get; set; }
        }
    }
}
