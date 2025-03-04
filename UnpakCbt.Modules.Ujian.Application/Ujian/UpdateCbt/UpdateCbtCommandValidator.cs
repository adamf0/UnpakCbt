﻿using FluentValidation;
using System.Text.RegularExpressions;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.UpdateCbt
{
    public sealed class UpdateCbtCommandValidator : AbstractValidator<UpdateCbtCommand>
    {
        private static readonly Regex GuidV4Regex = new(
            @"^[0-9a-f]{8}-[0-9a-f]{4}-4[0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static bool BeValidGuidV4(Guid guid)
        {
            return GuidV4Regex.IsMatch(guid.ToString());
        }
        public UpdateCbtCommandValidator() 
        {
            RuleFor(c => c.Uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.")
                .Must(BeValidGuidV4).WithMessage("'Uuid' harus dalam format UUID v4 yang valid.");

            RuleFor(c => c.NoReg)
                .NotEmpty().WithMessage("'NoReg' tidak boleh kosong.");

            RuleFor(c => c.IdJadwalUjian)
                .NotEmpty().WithMessage("'IdJadwalUjian' tidak boleh kosong.")
                .Must(BeValidGuidV4).WithMessage("'IdJadwalUjian' harus dalam format UUID v4 yang valid.");

            RuleFor(c => c.IdJawabanBenar)
                .NotEmpty().WithMessage("'IdJawabanBenar' tidak boleh kosong.")
                .Must(BeValidGuidV4).WithMessage("'IdJawabanBenar' harus dalam format UUID v4 yang valid.");

        }
    }
}
