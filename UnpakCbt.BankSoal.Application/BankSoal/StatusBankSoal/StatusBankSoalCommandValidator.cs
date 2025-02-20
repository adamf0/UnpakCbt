using FluentValidation;
using System;
using System.Collections.Generic;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.StatusBankSoal
{
    public sealed class StatusBankSoalCommandValidator : AbstractValidator<StatusBankSoalCommand>
    {
        public StatusBankSoalCommandValidator() 
        {
            RuleFor(c => c.Uuid)
                .NotEmpty().WithMessage("'Uuid' harus diisi.");

            RuleFor(c => c.Status)
                .NotEmpty().WithMessage("'Status' tidak boleh kosong.")
                .Must(c => c=="non-active" || c=="active").WithMessage("'Status' hanya boleh non-active dan active.");
        }
    }
}
