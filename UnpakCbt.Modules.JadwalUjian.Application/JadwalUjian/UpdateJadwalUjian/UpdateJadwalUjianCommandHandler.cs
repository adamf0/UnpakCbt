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

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.UpdateJadwalUjian
{
    internal sealed class UpdateJadwalUjianCommandHandler(
    IJadwalUjianRepository bankSoalRepository,
    IUnitOfWork unitOfWork,
    IBankSoalApi bankSoalApi)
    : ICommandHandler<UpdateJadwalUjianCommand>
    {
        public async Task<Result> Handle(UpdateJadwalUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.JadwalUjian.JadwalUjian? existingJadwalUjian = await bankSoalRepository.GetAsync(request.Uuid, cancellationToken);

            if (existingJadwalUjian is null)
            {
                Result.Failure(JadwalUjianErrors.NotFound(request.Uuid));
            }

            BankSoalResponse? bankSoal = await bankSoalApi.GetAsync(request.IdBankSoal, cancellationToken);

            if (bankSoal is null)
            {
                return Result.Failure<Guid>(BankSoalErrors.NotFound(request.IdBankSoal));
            }

            Result<Domain.JadwalUjian.JadwalUjian> asset = Domain.JadwalUjian.JadwalUjian.Update(existingJadwalUjian!)
                         .ChangeDeskripsi(request.Deskripsi)
                         .ChangeKuota(request.Kuota)
                         .ChangeTanggal(request.Tanggal)
                         .ChangeJamMulai(request.JamMulai)
                         .ChangeJamAkhir(request.JamAkhir)
                         .ChangeBankSoal(int.Parse(bankSoal.Id))
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
