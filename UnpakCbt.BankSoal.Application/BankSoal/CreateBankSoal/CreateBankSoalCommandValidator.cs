using FluentValidation;
using UnpakCbt.Common.Application.Security;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.CreateBankSoal
{
    public sealed class CreateBankSoalCommandValidator : AbstractValidator<CreateBankSoalCommand>
    {
        public CreateBankSoalCommandValidator() 
        {
            RuleFor(c => c.Judul)
                .NotEmpty().WithMessage("'Judul' tidak boleh kosong.");
        }
    }
}
