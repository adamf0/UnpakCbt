using System.ComponentModel.DataAnnotations.Schema;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal
{
    public sealed record BankSoalResponse
    {
        public string Uuid { get; set; }
        public string Judul { get; set; } = default!;
        [Column(TypeName = "TEXT")]
        public string Rule { get; set; } = "{}";
        public string Status { get; set; }
        public int JadwalTerhubung { get; set; }
    }
}
