using FluentValidation;
using System.Text.RegularExpressions;
using UnpakCbt.Common.Application.Security;

namespace UnpakCbt.Modules.Account.Application.Account.UpdateAccount
{
    public sealed class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        private static readonly Regex GuidV4Regex = new(
            @"^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        string SpecialCharacterPattern = @"[!@#$%^&*()_+\-=\[\]{}|;:'"",.<>?/\\]";
        string NumberPattern = @"^[0-9]+$";

        private static bool BeValidGuidV4(Guid guid)
        {
            return GuidV4Regex.IsMatch(guid.ToString());
        }
        private bool detectXss(string value)
        {
            return Xss.Check(value) != Xss.SanitizerType.CLEAR;
        }

        public UpdateAccountCommandValidator()
        {
            RuleFor(c => c.Uuid)
                .NotEmpty().WithMessage("'Uuid' harus diisi.")
                .Must(BeValidGuidV4).WithMessage("'Uuid' harus dalam format UUID v4 yang valid.");

            RuleFor(c => c.Username)
                .NotEmpty().WithMessage("'Username' tidak boleh kosong.")
                .Must(detectXss).WithMessage("'Username' terserang xss");

            /*RuleFor(c => c.Password)
                .NotEmpty().WithMessage("'Password' tidak boleh kosong.")
                .MinimumLength(8).WithMessage("'Password' harus memiliki minimal 8 karakter.")
                .Matches(SpecialCharacterPattern).WithMessage("'Password' harus mengandung minimal satu karakter spesial.")
                .Matches(NumberPattern).WithMessage("'Password' harus mengandung minimal satu angka.");*/

            RuleFor(c => c.Level)
                .NotEmpty().WithMessage("'Level' tidak boleh kosong.")
                .Must(detectXss).WithMessage("'Level' terserang xss")
                .Equal("admin").WithMessage(c => $"'Level' dengan nilai {c.Level} tidak dikenali sistem");
        }
    }
}
