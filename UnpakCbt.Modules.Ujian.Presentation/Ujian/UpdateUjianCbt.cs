using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.Ujian.Application.Ujian.UpdateCbt;

namespace UnpakCbt.Modules.Ujian.Presentation.Ujian
{
    internal static class UpdateUjianCbt
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("Ujian/Cbt", async (UpdateUjianCbtRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new UpdateCbtCommand(
                    request.Id,
                    request.NoReg,
                    request.IdJadwalUjian,
                    request.IdJawabanBenar
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.Ujian);
        }

        internal sealed class UpdateUjianCbtRequest
        {
            public Guid Id { get; set; }
            public string NoReg { get; set; }
            public Guid IdJadwalUjian { get; set; }
            public Guid IdJawabanBenar { get; set; }
        }
    }
}
