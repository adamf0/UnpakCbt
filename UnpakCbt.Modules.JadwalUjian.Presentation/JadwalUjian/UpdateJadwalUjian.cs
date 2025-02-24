using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.UpdateJadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Presentation.JadwalUjian
{
    internal static class UpdateJadwalUjian
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("JadwalUjian", async (UpdateJadwalUjianRequest request, ISender sender) =>
            {
                if (!SecurityCheck.NotContainInvalidCharacters(request.Id))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "Id mengandung karakter berbahaya"))));
                }
                if (!SecurityCheck.NotContainInvalidCharacters(request.IdBankSoal))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdBankSoal mengandung karakter berbahaya"))));
                }

                int kouta;
                if (!int.TryParse(request.Kouta, out kouta))
                {
                    kouta = 0;
                }

                Result result = await sender.Send(new UpdateJadwalUjianCommand(
                    Guid.Parse(request.Id),
                    request.Deskripsi,
                    kouta,
                    request.Tanggal,
                    request.JamMulai,
                    request.JamAkhir,
                    Guid.Parse(request.IdBankSoal)
                    )
                );

                return result.Match(() => Results.Ok(), ApiResults.Problem);
            }).WithTags(Tags.JadwalUjian);
        }

        internal sealed class UpdateJadwalUjianRequest
        {
            public string Id { get; set; }
            public string? Deskripsi { get; set; }

            public string Kouta { get; set; }
            public string Tanggal { get; set; }
            public string JamMulai { get; set; }
            public string JamAkhir { get; set; }
            public string IdBankSoal { get; set; }
        }
    }
}
