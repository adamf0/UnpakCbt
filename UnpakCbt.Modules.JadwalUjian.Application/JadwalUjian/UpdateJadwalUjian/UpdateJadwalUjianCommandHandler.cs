using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Application.Abstractions.Data;
using UnpakCbt.Modules.BankSoal.PublicApi;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;
using System.Globalization;
using Microsoft.Extensions.Logging;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.DeleteJadwalUjian;
using MediatR;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.UpdateJadwalUjian
{
    internal sealed class UpdateJadwalUjianCommandHandler(
    ICounterRepository counterRepository,
    IJadwalUjianRepository bankSoalRepository,
    IUnitOfWork unitOfWork,
    IBankSoalApi bankSoalApi,
    ILogger<UpdateJadwalUjianCommand> logger)
    : ICommandHandler<UpdateJadwalUjianCommand>
    {
        public async Task<Result> Handle(UpdateJadwalUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.JadwalUjian.JadwalUjian? existingJadwalUjian = await bankSoalRepository.GetAsync(request.Uuid, cancellationToken);
            BankSoalResponse? bankSoal = await bankSoalApi.GetAsync(request.IdBankSoal, cancellationToken);
            Result? result = checkData(logger, request.Uuid, existingJadwalUjian, request.IdBankSoal, bankSoal);
            if (result!=null) { 
                return result;
            }
            //checkFormat(request.Tanggal, request.JamMulai, request.JamAkhir);

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
                logger.LogError("domain bisnis JadwalUjian tidak sesuai aturan");
                return Result.Failure<Guid>(asset.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"berhasil update JadwalUjian dengan referensi uuid {request.Uuid}");

            result = checkFormat(logger, existingJadwalUjian.Tanggal, existingJadwalUjian.JamMulai, existingJadwalUjian.JamAkhir, true);
            if (result != null)
            {
                return result;
            }

            if (isTimeChanged(request.Tanggal, request.JamMulai, request.JamAkhir, existingJadwalUjian))
            {
                var timeToExpire = GetTimeToExpire(request.Tanggal, request.JamMulai);
                string key = $"counter_{request.Uuid}";

                await counterRepository.ResetCounterAsync(key, 0, timeToExpire);
                logger.LogInformation($"key {key} berhasil di reset ke 0");
            }

            //[PR]            
            //tahap 1: reset data ujian_cbt, insert ujian_cbt 

            //tahap 2: reset data ujian_cbt, insert ujian_cbt background job

            return Result.Success();
        }

        private Result? checkData(ILogger logger, Guid id, Domain.JadwalUjian.JadwalUjian? existingJadwalUjian, Guid IdBankSoal, BankSoalResponse? bankSoal) {
            if (existingJadwalUjian is null)
            {
                logger.LogError($"JadwalUjian dengan referensi Uuid {id} tidak ditemukan");
                return Result.Failure(JadwalUjianErrors.NotFound(id));
            }
            if (bankSoal is null)
            {
                logger.LogError($"BankSoal dengan referensi Uuid {IdBankSoal} tidak ditemukan");
                return Result.Failure<Guid>(BankSoalErrors.NotFound(IdBankSoal));
            }

            return null;
        }
        private Result? checkFormat(ILogger logger, string Tanggal, string JamMulai, string JamAkhir, bool isExisting = false)
        {
            //check valid format date time in 
            if (!DateTime.TryParseExact(Tanggal + " " + JamMulai, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var mulai))
            {
                logger.LogError($"gagal parse jam mulai JadwalUjian");
                return Result.Failure<Guid>(JadwalUjianErrors.InvalidScheduleFormat(!isExisting? "start": "existing start"));
            }

            if (!DateTime.TryParseExact(Tanggal + " " + JamAkhir, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var akhir))
            {
                logger.LogError($"gagal parse jam akhir JadwalUjian");
                return Result.Failure<Guid>(JadwalUjianErrors.InvalidScheduleFormat(!isExisting ? "end": "existing end"));
            }

            return null;
        }

        private static bool isTimeChanged(string Tanggal, string JamMulai, string JamAkhir, Domain.JadwalUjian.JadwalUjian existingJadwalUjian)
        {
            return TryParseDateTime(Tanggal, JamMulai, out var mulai) &&
                   TryParseDateTime(Tanggal, JamAkhir, out var akhir) &&
                   TryParseDateTime(existingJadwalUjian.Tanggal, existingJadwalUjian.JamMulai, out var existingMulai) &&
                   TryParseDateTime(existingJadwalUjian.Tanggal, existingJadwalUjian.JamAkhir, out var existingAkhir) &&
                   (mulai != existingMulai || akhir != existingAkhir);
        }

        private static TimeSpan GetTimeToExpire(string Tanggal, string JamMulai)
        {
            return TryParseDateTime(Tanggal, JamMulai, out var mulai)
                ? mulai - DateTime.UtcNow
                : TimeSpan.Zero;
        }

        private static bool TryParseDateTime(string tanggal, string jam, out DateTime result)
        {
            return DateTime.TryParseExact($"{tanggal} {jam}", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }
    }
}
