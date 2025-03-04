using FluentValidation;
using System.Globalization;
using System.Text.RegularExpressions;

namespace UnpakCbt.Modules.Laporan.Application.Laporan.GetAllLaporanLulus
{
    public sealed class GetAllLaporanLulusQueryValidator : AbstractValidator<GetAllLaporanLulusQuery>
    {
        private readonly Regex GuidV4Regex = new(
            @"^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private bool BeValidDate(string tanggal) =>
            DateTime.TryParseExact(tanggal, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

        private bool BeValidGuidV4(string? guid) =>
            !string.IsNullOrEmpty(guid) && GuidV4Regex.IsMatch(guid);

        public GetAllLaporanLulusQueryValidator()
        {
            RuleFor(c => c.UuidJadwalUjian)
                .Must(BeValidGuidV4).WithMessage("'UuidJadwalUjian' harus dalam format UUID v4 yang valid.")
                .When(c => !string.IsNullOrEmpty(c.UuidJadwalUjian)); // Hanya validasi jika Id diisi

            RuleFor(c => c.TanggalMulai)
                .Must(BeValidDate).WithMessage("'TanggalMulai' harus dalam format yyyy-MM-dd.")
                .When(c => !string.IsNullOrEmpty(c.TanggalMulai)); // Hanya validasi jika TanggalMulai diisi

            RuleFor(c => c.TanggalAkhir)
                .Must(BeValidDate).WithMessage("'TanggalAkhir' harus dalam format yyyy-MM-dd.")
                .When(c => !string.IsNullOrEmpty(c.TanggalAkhir)); // Hanya validasi jika TanggalAkhir diisi

            RuleFor(c => c)
                .Must(c => !string.IsNullOrWhiteSpace(c.UuidJadwalUjian) || (!string.IsNullOrWhiteSpace(c.TanggalMulai) || !string.IsNullOrWhiteSpace(c.TanggalAkhir)))
                .WithMessage("Minimal satu dari 'UuidJadwalUjian' atau 'Tanggal' harus diisi.");
        }
    }
}
