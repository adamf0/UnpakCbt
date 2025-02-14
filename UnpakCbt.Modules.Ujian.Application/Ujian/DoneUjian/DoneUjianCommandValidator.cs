using FluentValidation;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.DoneUjian
{
    public sealed class DoneUjianCommandValidator : AbstractValidator<DoneUjianCommand>
    {
        public DoneUjianCommandValidator() 
        {
            RuleFor(c => c.uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.");

            RuleFor(c => c.NoReg)
                .NotEmpty().WithMessage("'NoReg' tidak boleh kosong.");
        }
    }
}
