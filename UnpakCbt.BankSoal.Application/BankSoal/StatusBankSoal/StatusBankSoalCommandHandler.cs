using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Application.Abstractions.Data;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.StatusBankSoal
{
    internal sealed class StatusBankSoalCommandHandler(
    IBankSoalRepository bankSoalRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<StatusBankSoalCommand>
    {
        public async Task<Result> Handle(StatusBankSoalCommand request, CancellationToken cancellationToken)
        {
            Domain.BankSoal.BankSoal? existingBankSoal = await bankSoalRepository.GetAsync(request.Uuid, cancellationToken);

            if (existingBankSoal is null)
            {
                Result.Failure(BankSoalErrors.NotFound(request.Uuid));
            }

            Result<Domain.BankSoal.BankSoal> asset = Domain.BankSoal.BankSoal.Update(existingBankSoal!)
                         .ChangeStatus(request.Status)
                         .Build();

            if (asset.IsFailure)
            {
                return Result.Failure<Guid>(asset.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
