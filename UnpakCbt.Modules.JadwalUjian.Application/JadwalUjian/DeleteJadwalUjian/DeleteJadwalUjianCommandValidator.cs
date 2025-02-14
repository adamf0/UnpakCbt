using FluentValidation;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.DeleteJadwalUjian
{
    public sealed class DeleteJadwalUjianCommandValidator : AbstractValidator<DeleteJadwalUjianCommand>
    {
        public DeleteJadwalUjianCommandValidator() 
        {
            RuleFor(c => c.uuid)
                .NotEmpty().WithMessage("'uuid' tidak boleh kosong.");
        }
    }
}
