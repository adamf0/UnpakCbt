using System.ComponentModel.DataAnnotations.Schema;

namespace UnpakCbt.Modules.Laporan.Application.Laporan.GetLaporanLulus
{
    public sealed record LaporanLulusResponse
    {
        public string Uuid { get; set; }
        public string NoReg { get; set; }
        public string Tanggal { get; set; }
        public string Deskripsi { get; set; }
        public string? Keputusan { get; set; }
        public string? TanggalRespon { get; set; }
    }
}
