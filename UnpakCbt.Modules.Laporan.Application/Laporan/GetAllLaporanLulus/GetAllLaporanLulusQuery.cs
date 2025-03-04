using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Modules.Laporan.Application.Laporan.GetLaporanLulus;

namespace UnpakCbt.Modules.Laporan.Application.Laporan.GetAllLaporanLulus
{
    public sealed record GetAllLaporanLulusQuery(string? UuidJadwalUjian, string? TanggalMulai, string? TanggalAkhir) : IQuery<List<LaporanLulusResponse>>;
}
