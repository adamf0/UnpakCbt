using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.StatusBankSoal
{
    public sealed class StatusBankSoalCommandValidator : AbstractValidator<StatusBankSoalCommand>
    {
        private static readonly Regex GuidV4Regex = new(
            @"^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static bool BeValidGuidV4(Guid guid)
        {
            return GuidV4Regex.IsMatch(guid.ToString());
        }
        public StatusBankSoalCommandValidator() 
        {
            RuleFor(c => c.Uuid)
                .NotEmpty().WithMessage("'Uuid' harus diisi.")
                .Must(BeValidGuidV4).WithMessage("'Uuid' harus dalam format UUID v4 yang valid.");

            RuleFor(c => c.Status)
                .NotEmpty().WithMessage("'Status' tidak boleh kosong.")
                .Must(c => c=="non-active" || c=="active").WithMessage("'Status' hanya boleh non-active dan active.");
        }
    }
}
