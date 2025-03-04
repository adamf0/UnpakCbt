
namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian
{
    public sealed record JadwalUjianActiveResponse
    {
        public string? Deskripsi { get; set; } = default!;
        public string Kouta { get; set; }
        public string TotalJoin { get; set; }
        public string Tanggal { get; set; }
        public string JamMulai { get; set; }
        public string JamAkhir { get; set; }
        public string StatusUjian { get; set; }
    }
}
