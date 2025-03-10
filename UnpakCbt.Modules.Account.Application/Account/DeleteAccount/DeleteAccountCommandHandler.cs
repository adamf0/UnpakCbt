using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Account.Application.Abstractions.Data;
using UnpakCbt.Modules.Account.Domain.Account;

namespace UnpakCbt.Modules.Account.Application.Account.DeleteAccount
{
    internal sealed class DeleteAccountCommandHandler(
    IAccountRepository AccountRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteAccountCommand> logger)
    : ICommandHandler<DeleteAccountCommand>
    {
        public async Task<Result> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            Domain.Account.Account? existingAccount = await AccountRepository.GetAsync(request.uuid, cancellationToken);

            if (existingAccount is null)
            {
                logger.LogError($"Account dengan referensi uuid {request.uuid} tidak ditemukan");
                return Result.Failure(AccountErrors.NotFound(request.uuid));
            }

            await AccountRepository.DeleteAsync(existingAccount!);
            logger.LogInformation("berhasil hapus Account dengan referensi uuid {uuid}", request.uuid);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
