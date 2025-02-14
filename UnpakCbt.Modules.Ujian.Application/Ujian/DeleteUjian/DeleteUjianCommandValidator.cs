using FluentValidation;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.DeleteUjian
{
    public sealed class DeleteUjianCommandValidator : AbstractValidator<DeleteUjianCommand>
    {
        public DeleteUjianCommandValidator() 
        {
            RuleFor(c => c.uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.");

        }
    }
}
