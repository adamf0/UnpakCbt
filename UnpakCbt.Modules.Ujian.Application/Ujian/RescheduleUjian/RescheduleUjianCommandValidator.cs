using FluentValidation;
using System.Text.RegularExpressions;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.RescheduleUjian
{
    public sealed class RescheduleUjianCommandValidator : AbstractValidator<RescheduleUjianCommand>
    {
        private static readonly Regex GuidV4Regex = new(
            @"^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static bool BeValidGuidV4(Guid guid)
        {
            return GuidV4Regex.IsMatch(guid.ToString());
        }
        public RescheduleUjianCommandValidator() 
        {
            RuleFor(c => c.NoReg)
                .NotEmpty().WithMessage("'NoReg' tidak boleh kosong.");

            RuleFor(c => c.prevIdJadwalUjian)
                .NotEmpty().WithMessage("'prevIdJadwalUjian' tidak boleh kosong.")
                .Must(BeValidGuidV4).WithMessage("'prevIdJadwalUjian' harus dalam format UUID v4 yang valid.");

            RuleFor(c => c.newIdJadwalUjian)
                .NotEmpty().WithMessage("'newIdJadwalUjian' tidak boleh kosong.")
                .Must(BeValidGuidV4).WithMessage("'newIdJadwalUjian' harus dalam format UUID v4 yang valid.");

            RuleFor(c => c)
                .Must(c => c.prevIdJadwalUjian != c.newIdJadwalUjian)
                .WithMessage("'prevIdJadwalUjian' dan 'newIdJadwalUjian' tidak boleh sama.");

        }
    }
}
