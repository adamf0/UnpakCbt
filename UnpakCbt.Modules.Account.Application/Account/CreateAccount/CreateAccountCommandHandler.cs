using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Account.Application.Abstractions.Data;
using UnpakCbt.Modules.Account.Domain.Account;

namespace UnpakCbt.Modules.Account.Application.Account.CreateAccount
{
    internal sealed class AuthenticationCommandHandler(
    IAccountRepository AccountRepository,
    IUnitOfWork unitOfWork,
    ILogger<authenticationCommand> logger)
    : ICommandHandler<authenticationCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(authenticationCommand request, CancellationToken cancellationToken)
        {
            int check = await AccountRepository.CountByUsernameAsync(request.Username);
            if (check>0)
            {
                logger.LogError($"username {request.Username} sudah ada");
                return Result.Failure<Guid>(AccountErrors.NotUnique(request.Username));
            }

            Result<Domain.Account.Account> result = Domain.Account.Account.Create(
                request.Username,
                request.Password,
                request.Level,
                "active"
            );

            if (result.IsFailure)
            {
                logger.LogError("domain bisnis Account tidak sesuai aturan");
                return Result.Failure<Guid>(result.Error);
            }

            AccountRepository.Insert(result.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"berhasil simpan Account dengan hasil uuid {result.Value.Uuid}");

            return result.Value.Uuid;
        }
    }
}
