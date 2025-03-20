using FluentValidation;
using System.Globalization;
using System.Text.RegularExpressions;
using UnpakCbt.Common.Application.Security;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.UpdateJadwalUjian
{
    public sealed class UpdateJadwalUjianCommandValidator : AbstractValidator<UpdateJadwalUjianCommand>
    {
        private static readonly Regex GuidV4Regex = new(
            @"^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static bool BeValidGuidV4(Guid guid)
        {
            return GuidV4Regex.IsMatch(guid.ToString());
        }

        public UpdateJadwalUjianCommandValidator() 
        {
            RuleFor(c => c.Uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.")
                .Must(BeValidGuidV4).WithMessage("'Uuid' harus dalam format UUID v4 yang valid.");

            RuleFor(c => c.Kuota)
                .NotEmpty().WithMessage("'Kuota' tidak boleh kosong.")
                .Must(kuota => kuota > 0 || kuota == -1)
                .WithMessage("'Kuota' harus lebih dari 0, kecuali -1 untuk tanpa batas.");

            RuleFor(c => c.Tanggal)
                .NotEmpty().WithMessage("'Tanggal' tidak boleh kosong.")
                .Must(BeValidDate).WithMessage("'Tanggal' harus dalam format yyyy-MM-dd.");

            RuleFor(c => c.JamMulai)
                .NotEmpty().WithMessage("'JamMulai' tidak boleh kosong.")
                .Must(BeValidTime).WithMessage("'JamMulai' harus dalam format HH:mm.");

            RuleFor(c => c.JamAkhir)
                .NotEmpty().WithMessage("'JamAkhir' tidak boleh kosong.")
                .Must(BeValidTime).WithMessage("'JamAkhir' harus dalam format HH:mm.")
                .GreaterThan(c => c.JamMulai).WithMessage("'JamAkhir' harus lebih besar dari 'JamMulai'.");

            RuleFor(c => c.IdBankSoal)
                .NotEmpty().WithMessage("'IdBankSoal' tidak boleh kosong.")
                .Must(BeValidGuidV4).WithMessage("'IdBankSoal' harus dalam format UUID v4 yang valid.");
        }

        private bool BeValidDate(string tanggal)
        {
            return DateTime.TryParseExact(tanggal, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        private bool BeValidTime(string waktu)
        {
            return TimeSpan.TryParseExact(waktu, "hh\\:mm", CultureInfo.InvariantCulture, out _);
        }
    }
}
