using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Application.Abstractions.Data;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.UpdateBankSoal
{
    internal sealed class UpdateBankSoalCommandHandler(
    IBankSoalRepository bankSoalRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateBankSoalCommand>
    {
        public async Task<Result> Handle(UpdateBankSoalCommand request, CancellationToken cancellationToken)
        {
            Domain.BankSoal.BankSoal? existingBankSoal = await bankSoalRepository.GetAsync(request.Uuid, cancellationToken);

            if (existingBankSoal is null)
            {
                Result.Failure(BankSoalErrors.NotFound(request.Uuid));
            }

            Result<Domain.BankSoal.BankSoal> asset = Domain.BankSoal.BankSoal.Update(existingBankSoal!)
                         .ChangeJudul(request.Judul)
                         .ChangeRule(request.Rule)
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
