using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.CreateJadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Presentation.JadwalUjian
{
    internal static class CreateJadwalUjian
    {
        [Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("JadwalUjian", async (CreateJadwalUjianRequest request, ISender sender) =>
            {
                if (!SecurityCheck.NotContainInvalidCharacters(request.IdBankSoal))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdBankSoal mengandung karakter berbahaya"))));
                }
                if (!SecurityCheck.isValidGuid(request.IdBankSoal))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "IdBankSoal harus Guid format"))));
                }

                int kouta;
                if (!int.TryParse(request.Kouta, out kouta))
                {
                    kouta = 0;
                }

                Result<Guid> result = await sender.Send(new CreateJadwalUjianCommand(
                    request.Deskripsi,
                    kouta,
                    request.Tanggal, 
                    request.JamMulai,
                    request.JamAkhir,
                    Guid.Parse(request.IdBankSoal)
                    )
                );

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.JadwalUjian).RequireAuthorization();
        }

        internal sealed class CreateJadwalUjianRequest
        {
            public string? Deskripsi { get; set; }

            public string Kouta { get; set; }
            public string Tanggal { get; set; }
            public string JamMulai { get; set; }
            public string JamAkhir { get; set; }
            public string IdBankSoal { get; set; }

        }
    }
}
