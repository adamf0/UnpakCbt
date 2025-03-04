using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Laporan.Application.Laporan.GetAllLaporanLulus;
using UnpakCbt.Modules.Laporan.Application.Laporan.GetLaporanLulus;

namespace UnpakCbt.Modules.Laporan.Presentation.Laporan
{
    internal class GetAllLaporanLulus
    {
        [Authorize]
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("Laporan/lulus", async (GetAllLaporanLulusRequest request, ISender sender, HttpContext context, TokenValidator tokenValidator) =>
            {
                var (isValid, error) = tokenValidator.ValidateToken(context);
                if (!isValid)
                {
                    return error;
                }

                if (!SecurityCheck.NotContainInvalidCharacters(request?.UuidJadwalUjian ?? ""))
                {
                    return Results.BadRequest(ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "                    request?.UuidJadwalUjian,\r\n mengandung karakter berbahaya"))));
                }

                Result<List<LaporanLulusResponse>> result = await sender.Send(new GetAllLaporanLulusQuery(
                    request?.UuidJadwalUjian,
                    request?.TanggalMulai,
                    request?.TanggalAkhir
                ));

                return result.Match(Results.Ok, ApiResults.Problem);
            }).WithTags(Tags.Laporan).RequireAuthorization();
        }
        internal sealed class GetAllLaporanLulusRequest
        {
            public string? UuidJadwalUjian { get; set; }

            public string? TanggalMulai { get; set; }
            public string? TanggalAkhir { get; set; }
        }
    }
}
