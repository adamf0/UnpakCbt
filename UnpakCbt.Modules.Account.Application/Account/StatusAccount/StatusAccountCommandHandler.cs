using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Account.Application.Abstractions.Data;
using UnpakCbt.Modules.Account.Domain.Account;

namespace UnpakCbt.Modules.Account.Application.Account.StatusAccount
{
    internal sealed class StatusAccountCommandHandler(
    IAccountRepository AccountRepository,
    IUnitOfWork unitOfWork,
    ILogger<StatusAccountCommand> logger)
    : ICommandHandler<StatusAccountCommand>
    {
        public async Task<Result> Handle(StatusAccountCommand request, CancellationToken cancellationToken)
        {
            Domain.Account.Account? existingAccount = await AccountRepository.GetAsync(request.Uuid, cancellationToken);

            if (existingAccount is null)
            {
                logger.LogError($"Account dengan referensi uuid {request.Uuid} tidak ditemukan");
                return Result.Failure(AccountErrors.NotFound(request.Uuid));
            }

            Result<Domain.Account.Account> asset = Domain.Account.Account.Update(existingAccount!)
                         .ChangeStatus(request.Status)
                         .Build();

            if (asset.IsFailure)
            {
                logger.LogError("domain bisnis Account tidak sesuai aturan");
                return Result.Failure<Guid>(asset.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("berhasil ubah status Account menjadi {status} dengan referensi uuid {uuid}", request.Status, request.Uuid);

            return Result.Success();
        }
    }
}
