using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using UnpakCbt.Common.Domain;
using UnpakCbt.Common.Presentation.ApiResults;
using UnpakCbt.Common.Presentation.Security;
using UnpakCbt.Modules.Laporan.Application.Laporan.GetAllLaporanLulus;
using UnpakCbt.Modules.Laporan.Application.Laporan.GetAllLaporanLulusTotal;
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
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", "request UuidJadwalUjian mengandung karakter berbahaya")));
                }

                if (string.IsNullOrEmpty(request?.Type)) {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", $"request type wajib ada")));
                }
                if (request?.Type.ToLower() != "total" && request?.Type.ToLower() != "list")
                {
                    return ApiResults.Problem(Result.Failure(Error.Problem("Request.Invalid", $"request type hanya menerima 'total' dan 'list'")));
                }

                if (request?.Type.ToLower() == "total")
                {
                    Result<LaporanLulusTotalResponse> result = await sender.Send(new GetAllLaporanLulusTotalQuery(
                        request?.UuidJadwalUjian,
                        request?.TanggalMulai,
                        request?.TanggalAkhir
                    ));

                    return result.Match(Results.Ok, ApiResults.Problem);
                }
                else {
                    Result<List<LaporanLulusResponse>> result = await sender.Send(new GetAllLaporanLulusQuery(
                        request?.UuidJadwalUjian,
                        request?.TanggalMulai,
                        request?.TanggalAkhir
                    ));

                    return result.Match(Results.Ok, ApiResults.Problem);
                }
            }).WithTags(Tags.Laporan).RequireAuthorization();
        }
        internal sealed class GetAllLaporanLulusRequest
        {
            public string? UuidJadwalUjian { get; set; }

            public string? TanggalMulai { get; set; }
            public string? TanggalAkhir { get; set; }
            public string Type { get; set; } = "total";
        }
    }
}
