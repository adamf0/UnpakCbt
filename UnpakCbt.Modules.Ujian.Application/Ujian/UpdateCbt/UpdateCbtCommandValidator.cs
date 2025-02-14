using FluentValidation;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.UpdateCbt
{
    public sealed class UpdateCbtCommandValidator : AbstractValidator<UpdateCbtCommand>
    {
        public UpdateCbtCommandValidator() 
        {
            RuleFor(c => c.Uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.");

            RuleFor(c => c.NoReg)
                .NotEmpty().WithMessage("'NoReg' tidak boleh kosong.");

            RuleFor(c => c.IdJadwalUjian)
                .NotEmpty().WithMessage("'IdJadwalUjian' tidak boleh kosong.");

            RuleFor(c => c.IdJawabanBenar)
                .NotEmpty().WithMessage("'IdJawabanBenar' tidak boleh kosong.");

        }
    }
}
