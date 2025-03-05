using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Laporan.Application.Laporan.GetAllLaporanLulusTotal
{
    public sealed record GetAllLaporanLulusTotalQuery(
        string? UuidJadwalUjian,
        string? TanggalMulai,
        string? TanggalAkhir
    ) : IQuery<LaporanLulusTotalResponse>;
}
