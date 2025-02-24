using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.UpdateTemplateJawaban
{
    public sealed class UpdateTemplateJawabanCommandValidator : AbstractValidator<UpdateTemplateJawabanCommand>
    {
        private static readonly Regex GuidV4Regex = new(
            @"^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static bool BeValidGuidV4(Guid guid)
        {
            return GuidV4Regex.IsMatch(guid.ToString());
        }
        public UpdateTemplateJawabanCommandValidator() 
        {
            
            RuleFor(c => c.Uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.")
                .Must(BeValidGuidV4).WithMessage("'Uuid' harus dalam format UUID v4 yang valid.");

            RuleFor(c => c.IdTemplateSoal)
                .NotEmpty().WithMessage("'IdTemplateSoal' tidak boleh kosong.")
                .Must(BeValidGuidV4).WithMessage("'IdTemplateSoal' harus dalam format UUID v4 yang valid.");

            RuleFor(c => c)
                .Must(c => !string.IsNullOrWhiteSpace(c.JawabanText) || !string.IsNullOrWhiteSpace(c.JawabanImg))
                .WithMessage("Minimal satu dari 'JawabanText' atau 'JawabanImg' harus diisi.");
        }
    }
}
