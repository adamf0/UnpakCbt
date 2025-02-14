using FluentValidation;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.CreateTemplatePertanyaan
{
    public sealed class CreateTemplatePertanyaanCommandValidator : AbstractValidator<CreateTemplatePertanyaanCommand>
    {
        public CreateTemplatePertanyaanCommandValidator() 
        {
            RuleFor(c => c.IdBankSoal)
                .NotEmpty().WithMessage("'IdBankSoal' tidak boleh kosong.");

            RuleFor(c => c.Tipe)
                .NotEmpty().WithMessage("'Tipe' tidak boleh kosong.");

            RuleFor(c => c)
                .Must(c => !string.IsNullOrWhiteSpace(c.Pertanyaan) || !string.IsNullOrWhiteSpace(c.Gambar))
                .WithMessage("Minimal satu dari 'Pertanyaan' atau 'Gambar' harus diisi.");

            RuleFor(c => c.State)
                .NotEmpty().WithMessage("'State' tidak boleh kosong.");

        }
    }
}
