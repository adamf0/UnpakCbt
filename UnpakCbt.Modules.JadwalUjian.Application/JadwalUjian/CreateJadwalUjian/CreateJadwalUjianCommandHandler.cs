using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Application.Abstractions.Data;
using UnpakCbt.Modules.BankSoal.PublicApi;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.CreateJadwalUjian
{
    internal sealed class CreateJadwalUjianCommandHandler(
    IJadwalUjianRepository bankSoalRepository,
    IUnitOfWork unitOfWork,
    IBankSoalApi bankSoalApi)
    : ICommandHandler<CreateJadwalUjianCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateJadwalUjianCommand request, CancellationToken cancellationToken)
        {
            BankSoalResponse? bankSoal = await bankSoalApi.GetAsync(request.IdBankSoal, cancellationToken);

            if (bankSoal is null)
            {
                return Result.Failure<Guid>(BankSoalErrors.NotFound(request.IdBankSoal));
            }

            Result<Domain.JadwalUjian.JadwalUjian> result = Domain.JadwalUjian.JadwalUjian.Create(
                request.Deskripsi,
                request.Kuota,
                request.Tanggal,
                request.JamMulai,
                request.JamAkhir,
                int.Parse(bankSoal.Id)
            );

            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }

            bankSoalRepository.Insert(result.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return result.Value.Uuid;
        }
    }
}
