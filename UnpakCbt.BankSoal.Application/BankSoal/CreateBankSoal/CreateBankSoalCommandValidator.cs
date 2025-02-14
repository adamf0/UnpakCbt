using FluentValidation;

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
