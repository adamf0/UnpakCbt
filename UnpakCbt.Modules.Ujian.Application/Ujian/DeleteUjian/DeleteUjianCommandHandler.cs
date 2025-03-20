using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.Ujian.Domain.Cbt;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.DeleteUjian
{
    internal sealed class DeleteUjianCommandHandler(
    Domain.Ujian.ICounterRepository counterRepository,
    IUjianRepository ujianRepository,
    ICbtRepository cbtRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteUjianCommand> logger)
    : ICommandHandler<DeleteUjianCommand>
    {
        public async Task<Result> Handle(DeleteUjianCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Received command with parameters: {@Request}", request);

            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetAsync(request.uuid, cancellationToken);
            logger.LogInformation("existingUjian: {@existingUjian}", existingUjian);
            if (existingUjian is null)
            {
                logger.LogError($"Ujian dengan referensi Uuid {request.uuid} tidak ditemukan");
                return Result.Failure(UjianErrors.NotFound(request.uuid));
            }
            if (existingUjian?.NoReg != request.NoReg)
            {
                logger.LogError($"Ujian dengan referensi Uuid {request.uuid} tidak untuk NoReg {request.NoReg}");
                return Result.Failure<Guid>(UjianErrors.IncorrectReferenceNoReg(request.uuid, request.NoReg));
            }

            string key = "counter_" + existingUjian.Uuid.ToString();
            bool checkKey = await counterRepository.KeyExistsAsync(key);
            int postCounter = await counterRepository.GetCounterAsync(key);
            if (checkKey && (postCounter - 1) <1) {
                return Result.Failure<Guid>(UjianErrors.FailDecrement(key));
            }

            await cbtRepository.DeleteAsync(existingUjian?.Id ?? 0);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await ujianRepository.DeleteAsync(existingUjian!);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            if (checkKey)
            {
                counterRepository.DecrementCounterAsync(key, null);
            }

            return Result.Success();
        }
    }
}
