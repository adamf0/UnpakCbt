using FluentValidation;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.RescheduleUjian
{
    public sealed class RescheduleUjianCommandValidator : AbstractValidator<RescheduleUjianCommand>
    {
        public RescheduleUjianCommandValidator() 
        {
            RuleFor(c => c.NoReg)
                .NotEmpty().WithMessage("'NoReg' tidak boleh kosong.");

            RuleFor(c => c.prevIdJadwalUjian)
                .NotEmpty().WithMessage("'prevIdJadwalUjian' tidak boleh kosong.");

            RuleFor(c => c.newIdJadwalUjian)
                .NotEmpty().WithMessage("'newIdJadwalUjian' tidak boleh kosong.");

            RuleFor(c => c)
                .Must(c => c.prevIdJadwalUjian != c.newIdJadwalUjian)
                .WithMessage("'prevIdJadwalUjian' dan 'newIdJadwalUjian' tidak boleh sama.");

        }
    }
}
