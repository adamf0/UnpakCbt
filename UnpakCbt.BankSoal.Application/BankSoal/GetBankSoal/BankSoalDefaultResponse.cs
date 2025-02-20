
namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal
{
    public sealed record BankSoalDefaultResponse
    {
        public string Id { get; set; }
        public string Uuid { get; set; }
        public string Judul { get; set; } = default!;
        public string Rule { get; set; } = "{}";
        public string Status { get; set; }
    }
}
