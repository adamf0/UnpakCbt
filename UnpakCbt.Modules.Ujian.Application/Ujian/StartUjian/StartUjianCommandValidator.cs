using FluentValidation;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.StartUjian
{
    public sealed class StartUjianCommandValidator : AbstractValidator<StartUjianCommand>
    {
        public StartUjianCommandValidator() 
        {
            RuleFor(c => c.NoReg)
                .NotEmpty().WithMessage("'NoReg' tidak boleh kosong.");

            RuleFor(c => c.uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.");

        }
    }
}
