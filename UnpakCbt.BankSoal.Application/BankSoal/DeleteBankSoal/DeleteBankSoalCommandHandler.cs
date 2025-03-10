using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Application.Abstractions.Data;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.CreateBankSoal;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.DeleteBankSoal
{
    internal sealed class DeleteBankSoalCommandHandler(
    IBankSoalRepository bankSoalRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteBankSoalCommand> logger)
    : ICommandHandler<DeleteBankSoalCommand>
    {
        public async Task<Result> Handle(DeleteBankSoalCommand request, CancellationToken cancellationToken)
        {
            Domain.BankSoal.BankSoal? existingBankSoal = await bankSoalRepository.GetAsync(request.uuid, cancellationToken);

            if (existingBankSoal is null)
            {
                logger.LogError("BankSoal dengan referensi uuid {uuid} tidak ditemukan", request.uuid);
                return Result.Failure(BankSoalErrors.NotFound(request.uuid));
            }

            await bankSoalRepository.DeleteAsync(existingBankSoal!);
            logger.LogInformation("berhasil hapus BankSoal dengan referensi uuid {uuid}", request.uuid);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
