using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Application.Abstractions.Data;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.DeleteBankSoal;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.StatusBankSoal
{
    internal sealed class StatusBankSoalCommandHandler(
    IBankSoalRepository bankSoalRepository,
    IUnitOfWork unitOfWork,
    ILogger<StatusBankSoalCommand> logger)
    : ICommandHandler<StatusBankSoalCommand>
    {
        public async Task<Result> Handle(StatusBankSoalCommand request, CancellationToken cancellationToken)
        {
            Domain.BankSoal.BankSoal? existingBankSoal = await bankSoalRepository.GetAsync(request.Uuid, cancellationToken);

            if (existingBankSoal is null)
            {
                logger.LogError($"BankSoal dengan referensi uuid {request.Uuid} tidak ditemukan");
                return Result.Failure(BankSoalErrors.NotFound(request.Uuid));
            }

            Result<Domain.BankSoal.BankSoal> asset = Domain.BankSoal.BankSoal.Update(existingBankSoal!)
                         .ChangeStatus(request.Status)
                         .Build();

            if (asset.IsFailure)
            {
                logger.LogError("domain bisnis BankSoal tidak sesuai aturan");
                return Result.Failure<Guid>(asset.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("berhasil ubah status BankSoal menjadi {status} dengan referensi uuid {uuid}", request.Status, request.Uuid);

            return Result.Success();
        }
    }
}
