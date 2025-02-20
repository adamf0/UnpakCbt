using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Application.Abstractions.Data;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.CreateBankSoal
{
    internal sealed class CreateBankSoalCommandHandler(
    IBankSoalRepository bankSoalRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateBankSoalCommand> logger)
    : ICommandHandler<CreateBankSoalCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateBankSoalCommand request, CancellationToken cancellationToken)
        {
            Result<Domain.BankSoal.BankSoal> result = Domain.BankSoal.BankSoal.Create(
                request.Judul,
                request.Rule,
                "non-active"
            );

            if (result.IsFailure)
            {
                logger.LogError("domain bisnis BankSoal tidak sesuai aturan");
                return Result.Failure<Guid>(result.Error);
            }

            bankSoalRepository.Insert(result.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"berhasil simpan BankSoal dengan hasil uuid {result.Value.Uuid}");

            return result.Value.Uuid;
        }
    }
}
