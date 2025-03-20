using FluentValidation;
using System.Text.RegularExpressions;
using UnpakCbt.Common.Application.Security;

namespace UnpakCbt.Modules.Account.Application.Account.CreateAccount
{
    public sealed class AuthenticationCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        string SpecialCharacterPattern = @"[!@#$%^&*()_+\-=\[\]{}|;:'"",.<>?/\\]";
        string NumberPattern = @"^[0-9]+$";

        private bool detectXss(string value)
        {
            return Xss.Check(value)!=Xss.SanitizerType.CLEAR;
        }
        public AuthenticationCommandValidator()
        {
            RuleFor(c => c.Username)
                .NotEmpty().WithMessage("'Username' tidak boleh kosong.")
                .Must(detectXss).WithMessage("'Username' terserang xss");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("'Password' tidak boleh kosong.")
                .Must(detectXss).WithMessage("'Password' terserang xss")
                .MinimumLength(8).WithMessage("'Password' harus memiliki minimal 8 karakter.")
                .Matches(SpecialCharacterPattern).WithMessage("'Password' harus mengandung minimal satu karakter spesial.");
                //.Matches(NumberPattern).WithMessage("'Password' harus mengandung minimal satu angka.");

            RuleFor(c => c.Level)
                .NotEmpty().WithMessage("'Level' tidak boleh kosong.")
                .Must(detectXss).WithMessage("'Level' terserang xss")
                .Equal("admin").WithMessage(c => $"'Level' dengan nilai {c.Level} tidak dikenali sistem");
        }
    }
}
