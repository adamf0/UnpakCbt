using MediatR;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian;
using IJadwalUjianApi = UnpakCbt.Modules.JadwalUjian.PublicApi.IJadwalUjianApi;
using JadwalUjianResponseApi = UnpakCbt.Modules.JadwalUjian.PublicApi.JadwalUjianResponse;

namespace UnpakCbt.Modules.JadwalUjian.Infrastructure.PublicApi
{
    internal sealed class JadwalUjianApi(ISender sender) : IJadwalUjianApi
    {
        public async Task<JadwalUjianResponseApi?> GetAsync(Guid JadwalUjianUuid, CancellationToken cancellationToken = default)
        {
            Result<JadwalUjianDefaultResponse> result = await sender.Send(new GetJadwalUjianDefaultQuery(JadwalUjianUuid), cancellationToken);

            if (result.IsFailure)
            {
                return null;
            }

            return new JadwalUjianResponseApi(
                result.Value.Id,
                result.Value.Uuid,
                result.Value.Deskripsi,
                int.Parse(result.Value.Kouta),
                result.Value.Tanggal,
                result.Value.JamMulai,
                result.Value.JamAkhir,
                int.Parse(result.Value.IdBankSoal)
            );
        }

        public async Task<JadwalUjianResponseApi?> GetByIdAsync(int? id, CancellationToken cancellationToken = default)
        {
            Result<JadwalUjianDefaultResponse> result = await sender.Send(new GetJadwalUjianDefaultByIdQuery(id), cancellationToken);

            if (result.IsFailure)
            {
                return null;
            }

            return new JadwalUjianResponseApi(
                result.Value.Id,
                result.Value.Uuid,
                result.Value.Deskripsi,
                int.Parse(result.Value.Kouta),
                result.Value.Tanggal,
                result.Value.JamMulai,
                result.Value.JamAkhir,
                int.Parse(result.Value.IdBankSoal)
            );
        }
    }
}
