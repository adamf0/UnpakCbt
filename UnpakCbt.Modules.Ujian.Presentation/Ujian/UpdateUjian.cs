using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.Ujian.Application.Ujian.UpdateUjian;
using UnpakCbt.Modules.Ujian.Presentation;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal static class UpdateUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("Ujian", async (UpdateUjianRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new UpdateUjianCommand(
                    request.Id,
                    request.NoReg,
                    request.IdJadwalUjian,
                    request.Status
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }

        internal sealed class UpdateUjianRequest
        {
            public Guid Id { get; set; }
            public string NoReg { get; set; }
            public Guid IdJadwalUjian { get; set; }
            public string Status { get; set; }
        }
    }
}
