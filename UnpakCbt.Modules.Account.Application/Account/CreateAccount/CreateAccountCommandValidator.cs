using FluentValidation;

namespace UnpakCbt.Modules.Account.Application.Account.CreateAccount
{
    public sealed class AuthenticationCommandValidator : AbstractValidator<authenticationCommand>
    {
        string SpecialCharacterPattern = @"[!@#$%^&*()_+\-=\[\]{}|;:'"",.<>?/\\]";
        string NumberPattern = @"^[0-9]+$";
        public AuthenticationCommandValidator()
        {
            RuleFor(c => c.Username)
                .NotEmpty().WithMessage("'Username' tidak boleh kosong.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("'Password' tidak boleh kosong.")
                .MinimumLength(8).WithMessage("'Password' harus memiliki minimal 8 karakter.")
                .Matches(SpecialCharacterPattern).WithMessage("'Password' harus mengandung minimal satu karakter spesial.");
                //.Matches(NumberPattern).WithMessage("'Password' harus mengandung minimal satu angka.");

            RuleFor(c => c.Level)
                .NotEmpty().WithMessage("'Level' tidak boleh kosong.")
                .Equal("admin").WithMessage(c => $"'Level' dengan nilai {c.Level} tidak dikenali sistem");
        }
    }
}
