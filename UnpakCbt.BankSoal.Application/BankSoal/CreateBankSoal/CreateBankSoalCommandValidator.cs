using FluentValidation;
using UnpakCbt.Common.Application.Security;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.CreateBankSoal
{
    public sealed class CreateBankSoalCommandValidator : AbstractValidator<CreateBankSoalCommand>
    {
        private bool detectXss(string value)
        {
            return Xss.Check(value) != Xss.SanitizerType.CLEAR;
        }
        public CreateBankSoalCommandValidator() 
        {
            RuleFor(c => c.Judul)
                .NotEmpty().WithMessage("'Judul' tidak boleh kosong.")
                .Must(detectXss).WithMessage("'Judul' terserang xss");
        }
    }
}
