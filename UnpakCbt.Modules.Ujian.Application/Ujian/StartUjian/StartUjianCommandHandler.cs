using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.StartUjian
{
    internal sealed class StartUjianCommandHandler(
    IUjianRepository ujianRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<StartUjianCommand>
    {
        public async Task<Result> Handle(StartUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetAsync(request.uuid, cancellationToken);
            if (existingUjian is null)
            {
                Result.Failure(UjianErrors.NotFound(request.uuid));
            }
            if (existingUjian?.NoReg != request.NoReg)
            {
                return Result.Failure<Guid>(UjianErrors.IncorrectReferenceNoReg(request.NoReg, existingUjian?.NoReg??"-"));
            }

            if (existingUjian?.Status == "active")
            {
                Result<Domain.Ujian.Ujian> prevUjian = Domain.Ujian.Ujian.Update(existingUjian!)
                         .ChangeStatus("start")
                         .Build();

                if (prevUjian.IsFailure)
                {
                    return Result.Failure<Guid>(prevUjian.Error);
                }
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return Result.Success();
        }
    }
}
