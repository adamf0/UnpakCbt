using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Application.Abstractions.Data;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.StatusBankSoal;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.UpdateBankSoal
{
    internal sealed class UpdateBankSoalCommandHandler(
    IBankSoalRepository bankSoalRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateBankSoalCommand> logger)
    : ICommandHandler<UpdateBankSoalCommand>
    {
        public async Task<Result> Handle(UpdateBankSoalCommand request, CancellationToken cancellationToken)
        {
            Domain.BankSoal.BankSoal? existingBankSoal = await bankSoalRepository.GetAsync(request.Uuid, cancellationToken);

            if (existingBankSoal is null)
            {
                logger.LogError($"BankSoal dengan referensi uuid {request.Uuid} tidak ditemukan");
                return Result.Failure(BankSoalErrors.NotFound(request.Uuid));
            }

            Result<Domain.BankSoal.BankSoal> asset = Domain.BankSoal.BankSoal.Update(existingBankSoal!)
                         .ChangeJudul(request.Judul)
                         .ChangeRule(request.Rule)
                         .Build();

            if (asset.IsFailure)
            {
                logger.LogError("domain bisnis BankSoal tidak sesuai aturan");
                return Result.Failure<Guid>(asset.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"berhasil update BankSoal dengan referensi uuid {request.Uuid}");

            return Result.Success();
        }
    }
}
