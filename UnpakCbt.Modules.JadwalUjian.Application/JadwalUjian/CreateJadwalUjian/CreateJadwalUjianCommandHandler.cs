using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Application.Abstractions.Data;
using UnpakCbt.Modules.BankSoal.PublicApi;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.CreateJadwalUjian
{
    internal sealed class CreateJadwalUjianCommandHandler(
    IJadwalUjianRepository bankSoalRepository,
    ICounterRepository counterRepository,
    IUnitOfWork unitOfWork,
    IBankSoalApi bankSoalApi,
    ILogger<CreateJadwalUjianCommand> logger)
    : ICommandHandler<CreateJadwalUjianCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateJadwalUjianCommand request, CancellationToken cancellationToken)
        {
            BankSoalResponse? bankSoal = await bankSoalApi.GetAsync(request.IdBankSoal, cancellationToken);

            if (bankSoal is null)
            {
                logger.LogError($"BankSoal dengan referensi id {request.IdBankSoal} tidak ditemukan");
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
                logger.LogError("domain bisnis JadwalUjian tidak sesuai aturan");
                return Result.Failure<Guid>(result.Error);
            }


            DateTime.TryParseExact(request.Tanggal + " " + request.JamMulai, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var mulai);

            DateTime.TryParseExact(request.Tanggal + " " + request.JamAkhir, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var akhir);

            TimeSpan timeToExpire = mulai - DateTime.UtcNow;

            bankSoalRepository.Insert(result.Value);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("berhasil simpan JadwalUjian dengan hasil Uuid {uuid}",result.Value.Uuid);

            string key = "counter_" + result.Value.Uuid.ToString();
            await counterRepository.ResetCounterAsync(key, 0, timeToExpire);
            logger.LogInformation("berhasil setup {key}", key);

            return result.Value.Uuid;
        }
    }
}
