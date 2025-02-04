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
using MediatR;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.UpdateJadwalUjian
{
    internal sealed class UpdateJadwalUjianCommandHandler(
    ICounterRepository counterRepository,
    IJadwalUjianRepository bankSoalRepository,
    IUnitOfWork unitOfWork,
    IBankSoalApi bankSoalApi)
    : ICommandHandler<UpdateJadwalUjianCommand>
    {
        public async Task<Result> Handle(UpdateJadwalUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.JadwalUjian.JadwalUjian? existingJadwalUjian = await bankSoalRepository.GetAsync(request.Uuid, cancellationToken);
            BankSoalResponse? bankSoal = await bankSoalApi.GetAsync(request.IdBankSoal, cancellationToken);
            checkData(request.Uuid, existingJadwalUjian, request.IdBankSoal, bankSoal);
            checkFormat(request.Tanggal, request.JamMulai, request.JamAkhir);

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

            checkFormat(existingJadwalUjian.Tanggal, existingJadwalUjian.JamMulai, existingJadwalUjian.JamAkhir, true);
            if (isTimeChanged(request.Tanggal, request.JamMulai, request.JamAkhir, existingJadwalUjian))
            {
                var timeToExpire = GetTimeToExpire(request.Tanggal, request.JamMulai);
                string key = $"counter_{request.Uuid}";
                await counterRepository.ResetCounterAsync(key, 0, timeToExpire);
            }

            //tahap 1: reset data ujian_cbt, insert ujian_cbt manual
            await unitOfWork.SaveChangesAsync(cancellationToken);
            //tahap 2: reset data ujian_cbt, insert ujian_cbt background job

            return Result.Success();
        }

        private Result? checkData(Guid id, Domain.JadwalUjian.JadwalUjian? existingJadwalUjian, Guid IdBankSoal, BankSoalResponse? bankSoal) {
            if (existingJadwalUjian is null)
            {
                Result.Failure(JadwalUjianErrors.NotFound(id));
            }
            if (bankSoal is null)
            {
                return Result.Failure<Guid>(BankSoalErrors.NotFound(IdBankSoal));
            }

            return null;
        }
        private Result? checkFormat(string Tanggal, string JamMulai, string JamAkhir, bool isExisting = false)
        {
            //check valid format date time in 
            if (!DateTime.TryParseExact(Tanggal + " " + JamMulai, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var mulai))
            {
                return Result.Failure<Guid>(JadwalUjianErrors.InvalidScheduleFormat(!isExisting? "start date": "existing start date"));
            }

            if (!DateTime.TryParseExact(Tanggal + " " + JamAkhir, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var akhir))
            {
                return Result.Failure<Guid>(JadwalUjianErrors.InvalidScheduleFormat(!isExisting ? "end date": "existing end date"));
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
