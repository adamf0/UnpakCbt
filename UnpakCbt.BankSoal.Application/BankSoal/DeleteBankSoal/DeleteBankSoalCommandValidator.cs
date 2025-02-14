using FluentValidation;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.DeleteBankSoal
{
    public sealed class DeleteBankSoalCommandValidator : AbstractValidator<DeleteBankSoalCommand>
    {
        public DeleteBankSoalCommandValidator() 
        {
            RuleFor(c => c.uuid)
                .NotEmpty().WithMessage("'Uuid' harus diisi.");
        }
    }
}
