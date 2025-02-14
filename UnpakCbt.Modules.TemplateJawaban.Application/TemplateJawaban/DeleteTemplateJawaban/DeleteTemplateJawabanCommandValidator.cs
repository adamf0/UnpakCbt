using FluentValidation;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.DeleteTemplateJawaban
{
    public sealed class DeleteTemplateJawabanCommandValidator : AbstractValidator<DeleteTemplateJawabanCommand>
    {
        public DeleteTemplateJawabanCommandValidator() 
        {
            
            RuleFor(c => c.uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.");
        }
    }
}
