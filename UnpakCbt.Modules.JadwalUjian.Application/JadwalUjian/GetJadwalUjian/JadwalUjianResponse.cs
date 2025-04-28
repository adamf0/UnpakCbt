
namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian
{
    public sealed record JadwalUjianResponse
    {
        public string Uuid { get; set; }
        public string? Deskripsi { get; set; } = default!;
        public string Kouta { get; set; }
        public string Tanggal { get; set; }
        public string JamMulai { get; set; }
        public string JamAkhir { get; set; }
        public string UuidBankSoal { get; set; }
        public string? UuidBankSoalTrial { get; set; }
    }
}
