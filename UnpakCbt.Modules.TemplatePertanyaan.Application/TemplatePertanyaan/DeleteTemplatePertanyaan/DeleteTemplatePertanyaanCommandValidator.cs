using FluentValidation;
using System.Text.RegularExpressions;
using UnpakCbt.Common.Application.Security;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.DeleteTemplatePertanyaan
{
    public sealed class DeleteTemplatePertanyaanCommandValidator : AbstractValidator<DeleteTemplatePertanyaanCommand>
    {
        private static readonly Regex GuidV4Regex = new(
            @"^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static bool BeValidGuidV4(Guid guid)
        {
            return GuidV4Regex.IsMatch(guid.ToString());
        }
        private bool detectXss(string value)
        {
            return Xss.Check(value) != Xss.SanitizerType.CLEAR;
        }
        public DeleteTemplatePertanyaanCommandValidator() 
        {
            RuleFor(c => c.uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.")
                .Must(BeValidGuidV4).WithMessage("'Uuid' harus dalam format UUID v4 yang valid.");

        }
    }
}
