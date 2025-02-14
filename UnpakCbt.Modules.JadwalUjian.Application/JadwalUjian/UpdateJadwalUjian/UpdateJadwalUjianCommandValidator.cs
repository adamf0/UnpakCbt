﻿using FluentValidation;
using System.Globalization;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.UpdateJadwalUjian
{
    public sealed class UpdateJadwalUjianCommandValidator : AbstractValidator<UpdateJadwalUjianCommand>
    {
        public UpdateJadwalUjianCommandValidator() 
        {
            RuleFor(c => c.Uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.");

            RuleFor(c => c.Kuota)
                .NotEmpty().WithMessage("'Kuota' tidak boleh kosong.")
                .Must(kuota => kuota > 0 || kuota == -1)
                .WithMessage("'Kuota' harus lebih dari 0, kecuali -1 untuk tanpa batas.");

            RuleFor(c => c.Tanggal)
                .NotEmpty().WithMessage("'Tanggal' tidak boleh kosong.")
                .Must(BeValidDate).WithMessage("'Tanggal' harus dalam format yyyy-MM-dd.");

            RuleFor(c => c.JamMulai)
                .NotEmpty().WithMessage("'JamMulai' tidak boleh kosong.")
                .Must(BeValidTime).WithMessage("'JamMulai' harus dalam format HH:mm.");

            RuleFor(c => c.JamAkhir)
                .NotEmpty().WithMessage("'JamAkhir' tidak boleh kosong.")
                .Must(BeValidTime).WithMessage("'JamAkhir' harus dalam format HH:mm.")
                .GreaterThan(c => c.JamMulai).WithMessage("'JamAkhir' harus lebih besar dari 'JamMulai'.");

            RuleFor(c => c.IdBankSoal)
                .NotEmpty().WithMessage("'IdBankSoal' tidak boleh kosong.");
        }

        private bool BeValidDate(string tanggal)
        {
            return DateTime.TryParseExact(tanggal, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        private bool BeValidTime(string waktu)
        {
            return TimeSpan.TryParseExact(waktu, "hh\\:mm", CultureInfo.InvariantCulture, out _);
        }
    }
}
