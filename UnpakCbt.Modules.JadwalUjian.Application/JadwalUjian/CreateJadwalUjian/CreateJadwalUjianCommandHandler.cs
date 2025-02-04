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
using System.Globalization;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.CreateJadwalUjian
{
    internal sealed class CreateJadwalUjianCommandHandler(
    IJadwalUjianRepository bankSoalRepository,
    ICounterRepository counterRepository,
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

            if (!DateTime.TryParseExact(request.Tanggal + " " + request.JamMulai, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var mulai))
            {
                return Result.Failure<Guid>(JadwalUjianErrors.InvalidScheduleFormat("start date"));
            }

            if (!DateTime.TryParseExact(request.Tanggal + " " + request.JamAkhir, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var akhir))
            {
                return Result.Failure<Guid>(JadwalUjianErrors.InvalidScheduleFormat("end date"));
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

            TimeSpan timeToExpire = mulai - DateTime.UtcNow;
            string key = "counter_" + result.Value.Uuid.ToString();

            bankSoalRepository.Insert(result.Value);
            await counterRepository.ResetCounterAsync(key, 0, timeToExpire);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return result.Value.Uuid;
        }
    }
}
