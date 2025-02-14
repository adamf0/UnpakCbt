using FluentValidation;
using System;
using System.Collections.Generic;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.UpdateBankSoal
{
    public sealed class UpdateBankSoalCommandValidator : AbstractValidator<UpdateBankSoalCommand>
    {
        public UpdateBankSoalCommandValidator() 
        {
            RuleFor(c => c.Uuid)
                .NotEmpty().WithMessage("'Uuid' harus diisi.");

            RuleFor(c => c.Judul)
                .NotEmpty().WithMessage("'Judul' tidak boleh kosong.");
        }
    }
}
