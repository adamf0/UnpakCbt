using FluentValidation;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.UpdateUjian
{
    public sealed class UpdateUjianCommandValidator : AbstractValidator<UpdateUjianCommand>
    {
        public UpdateUjianCommandValidator() 
        {
            RuleFor(c => c.Uuid)
                .NotEmpty().WithMessage("'Uuid' tidak boleh kosong.");

            RuleFor(c => c.NoReg)
                .NotEmpty().WithMessage("'NoReg' tidak boleh kosong.");

            RuleFor(c => c.IdJadwalUjian)
                .NotEmpty().WithMessage("'IdJadwalUjian' tidak boleh kosong.");

            RuleFor(c => c.Status)
                .NotEmpty().WithMessage("'Status' tidak boleh kosong.");

        }
    }
}
