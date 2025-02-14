using FluentValidation;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.UpdateTemplatePertanyaan
{
    public sealed class UpdateTemplatePertanyaanCommandValidator : AbstractValidator<UpdateTemplatePertanyaanCommand>
    {
        public UpdateTemplatePertanyaanCommandValidator() 
        {
            RuleFor(c => c.Uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.");

            RuleFor(c => c.IdBankSoal)
                .NotEmpty().WithMessage("'IdBankSoal' tidak boleh kosong.");

            RuleFor(c => c.Tipe)
                .NotEmpty().WithMessage("'Tipe' tidak boleh kosong.");

            RuleFor(c => c)
                .Must(c => !string.IsNullOrWhiteSpace(c.Pertanyaan) || !string.IsNullOrWhiteSpace(c.Gambar))
                .WithMessage("Minimal satu dari 'Pertanyaan' atau 'Gambar' harus diisi.");

            RuleFor(c => c.Bobot)
                .NotEmpty().WithMessage("'Bobot' tidak boleh kosong.");

        }
    }
}
