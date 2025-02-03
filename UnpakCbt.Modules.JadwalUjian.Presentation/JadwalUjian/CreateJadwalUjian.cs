using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.Encodings.Web;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.CreateJadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Presentation;

namespace UnpakCbt.Modules.JadwalUjian.Presentation.JadwalUjian
{
    internal static class CreateJadwalUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("JadwalUjian", async (CreateJadwalUjianRequest request, ISender sender) =>
            {
                Result<Guid> result = await sender.Send(new CreateJadwalUjianCommand(
                    request.Deskripsi,
                    int.Parse(request.Kouta),
                    request.Tanggal, 
                    request.JamMulai,
                    request.JamAkhir,
                    request.IdBankSoal
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.JadwalUjian);
        }

        internal sealed class CreateJadwalUjianRequest
        {
            public string? Deskripsi { get; set; }

            public string Kouta { get; set; }
            public string Tanggal { get; set; }
            public string JamMulai { get; set; }
            public string JamAkhir { get; set; }
            public Guid IdBankSoal { get; set; }

        }
    }
}
