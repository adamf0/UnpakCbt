using FluentValidation;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.CreateTemplateJawaban
{
    public sealed class CreateTemplateJawabanCommandValidator : AbstractValidator<CreateTemplateJawabanCommand>
    {
        public CreateTemplateJawabanCommandValidator() 
        {
            RuleFor(c => c.IdTemplateSoal)
                .NotEmpty().WithMessage("'IdTemplateSoal' tidak boleh kosong.");

            RuleFor(c => c)
                .Must(c => !string.IsNullOrWhiteSpace(c.JawabanText) || !string.IsNullOrWhiteSpace(c.JawabanImg))
                .WithMessage("Minimal satu dari 'JawabanText' atau 'JawabanImg' harus diisi.");
        }
    }
}
