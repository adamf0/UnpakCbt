using FluentValidation;
using System.Text.RegularExpressions;
using UnpakCbt.Common.Application.Security;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.CreateTemplatePertanyaan
{
    public sealed class CreateTemplatePertanyaanCommandValidator : AbstractValidator<CreateTemplatePertanyaanCommand>
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

        public CreateTemplatePertanyaanCommandValidator() 
        {
            RuleFor(c => c.IdBankSoal)
                .NotEmpty().WithMessage("'IdBankSoal' tidak boleh kosong.")
                .Must(BeValidGuidV4).WithMessage("'IdBankSoal' harus dalam format UUID v4 yang valid.");

            RuleFor(c => c.Tipe)
                .NotEmpty().WithMessage("'Tipe' tidak boleh kosong.")
                .Must(detectXss).WithMessage("'Tipe' terserang xss");

            //RuleFor(c => c)
            //    .Must(c => !string.IsNullOrWhiteSpace(c.Pertanyaan) || !string.IsNullOrWhiteSpace(c.Gambar))
            //    .WithMessage("Minimal satu dari 'Pertanyaan' atau 'Gambar' harus diisi.");

            RuleFor(c => c.State)
                .NotEmpty().WithMessage("'State' tidak boleh kosong.")
                .Must(detectXss).WithMessage("'State' terserang xss");

        }
    }
}
