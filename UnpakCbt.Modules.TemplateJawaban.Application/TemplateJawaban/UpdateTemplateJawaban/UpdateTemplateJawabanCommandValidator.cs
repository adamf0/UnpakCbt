using FluentValidation;
using System;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.UpdateTemplateJawaban
{
    public sealed class UpdateTemplateJawabanCommandValidator : AbstractValidator<UpdateTemplateJawabanCommand>
    {
        public UpdateTemplateJawabanCommandValidator() 
        {
            
            RuleFor(c => c.Uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.");

            RuleFor(c => c.IdTemplateSoal)
                .NotEmpty().WithMessage("'IdTemplateSoal' tidak boleh kosong.");

            RuleFor(c => c)
                .Must(c => !string.IsNullOrWhiteSpace(c.JawabanText) || !string.IsNullOrWhiteSpace(c.JawabanImg))
                .WithMessage("Minimal satu dari 'JawabanText' atau 'JawabanImg' harus diisi.");
        }
    }
}
