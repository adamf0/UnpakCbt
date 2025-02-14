using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.DeleteUjian
{
    internal sealed class DeleteUjianCommandHandler(
    IUjianRepository ujianRepository,
    ICbtRepository cbtRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteUjianCommand>
    {
        public async Task<Result> Handle(DeleteUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetAsync(request.uuid, cancellationToken);

            if (existingUjian is null)
            {
                return Result.Failure(UjianErrors.NotFound(request.uuid));
            }
            if (existingUjian?.NoReg != request.NoReg)
            {
                return Result.Failure<Guid>(UjianErrors.IncorrectReferenceNoReg(request.NoReg, existingUjian?.NoReg??"-"));
            }

            await cbtRepository.DeleteAsync(existingUjian?.Id ?? 0);
            await ujianRepository.DeleteAsync(existingUjian!);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
