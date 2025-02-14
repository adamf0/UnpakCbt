using FluentValidation;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.CancelUjian
{
    public sealed class CancelUjianCommandValidator : AbstractValidator<CancelUjianCommand>
    {
        public CancelUjianCommandValidator() 
        {
            RuleFor(c => c.uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.");

        }
    }
}
