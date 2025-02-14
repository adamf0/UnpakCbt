using FluentValidation;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.DeleteTemplatePertanyaan
{
    public sealed class DeleteTemplatePertanyaanCommandValidator : AbstractValidator<DeleteTemplatePertanyaanCommand>
    {
        public DeleteTemplatePertanyaanCommandValidator() 
        {
            RuleFor(c => c.uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.");

        }
    }
}
