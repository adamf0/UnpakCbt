using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.UpdateJadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Presentation.JadwalUjian
{
    internal static class UpdateJadwalUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("JadwalUjian", async (UpdateJadwalUjianRequest request, ISender sender) =>
            {
                Result result = await sender.Send(new UpdateJadwalUjianCommand(
                    request.Id,
                    request.Deskripsi,
                    int.Parse(request.Kouta),
                    request.Tanggal,
                    request.JamMulai,
                    request.JamAkhir,
                    request.IdBankSoal
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.JadwalUjian);
        }

        internal sealed class UpdateJadwalUjianRequest
        {
            public Guid Id { get; set; }
            public string? Deskripsi { get; set; }

            public string Kouta { get; set; }
            public string Tanggal { get; set; }
            public string JamMulai { get; set; }
            public string JamAkhir { get; set; }
            public Guid IdBankSoal { get; set; }
        }
    }
}
