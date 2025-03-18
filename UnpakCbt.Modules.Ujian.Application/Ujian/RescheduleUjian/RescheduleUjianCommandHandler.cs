using System.Globalization;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.PublicApi;
using UnpakCbt.Modules.TemplatePertanyaan.PublicApi;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.Ujian.Domain.Cbt;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.RescheduleUjian
{
    internal sealed class RescheduleUjianCommandHandler( 
    Domain.Ujian.ICounterRepository counterRepository,
    IUjianRepository ujianRepository,
    ICbtRepository cbtRepository,
    IUnitOfWork unitOfWork,
    IJadwalUjianApi jadwalUjianApi,
    ITemplatePertanyaanApi templatePertanyaanApi)
    : ICommandHandler<RescheduleUjianCommand,Guid>
    {
        public async Task<Result<Guid>> Handle(RescheduleUjianCommand request, CancellationToken cancellationToken)
        {
            /*int totalActiveUjian = await ujianRepository.GetCountJadwalActiveAsync(request.NoReg); //active, start, done
            if (totalActiveUjian > 0)
            {
                return Result.Failure<Guid>(UjianErrors.ActiveExam(request.NoReg));
            }*/

            JadwalUjianResponse? existingPrevJadwalUjian = await jadwalUjianApi.GetAsync(request.prevIdJadwalUjian, cancellationToken);
            if (existingPrevJadwalUjian is null)
            {
                Result.Failure(Domain.JadwalUjian.JadwalUjianErrors.NotFound(request.prevIdJadwalUjian));
            }

            JadwalUjianResponse? newJadwalUjian = await jadwalUjianApi.GetAsync(request.newIdJadwalUjian, cancellationToken);
            if (newJadwalUjian is null)
            {
                Result.Failure(Domain.JadwalUjian.JadwalUjianErrors.NotFound(request.newIdJadwalUjian));
            }


            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetByNoRegWithJadwalAsync(request.NoReg, int.Parse(existingPrevJadwalUjian.Id), cancellationToken);

            if (existingUjian is null)
            {
                Result.Failure(UjianErrors.NoRegNotEmpty());
            }
            if (existingUjian?.Status == "cancel")
            {
                return Result.Failure<Guid>(UjianErrors.ScheduleExamCancelExam());
            }
            if (existingUjian?.Status == "done") {
                return Result.Failure<Guid>(UjianErrors.ScheduleExamDoneExam());
            }
            if (existingUjian?.Status == "start") {
                return Result.Failure<Guid>(UjianErrors.ScheduleExamStartExam());
            }

            string key = "counter_" + request.newIdJadwalUjian.ToString();
            bool checkNewKey = await counterRepository.KeyExistsAsync(key);
            int kuotaNewJadwal = (newJadwalUjian?.Kuota ?? 0);
            int totalData = await ujianRepository.GetCountJadwalByJadwalUjianAsync(int.Parse(newJadwalUjian?.Id ?? "0"));

            if (kuotaNewJadwal > 0) {
                int postCounter = await counterRepository.GetCounterAsync(key) + 1;
                if (totalData > kuotaNewJadwal)
                { //check total data tabel
                    return Result.Failure<Guid>(UjianErrors.QuotaExhausted2(postCounter.ToString(), kuotaNewJadwal.ToString()));
                }
                if (checkNewKey && postCounter > kuotaNewJadwal) //check data redis
                {
                    return Result.Failure<Guid>(UjianErrors.QuotaExhausted(postCounter.ToString(), kuotaNewJadwal.ToString()));
                }
            }

            checkData(request.newIdJadwalUjian, newJadwalUjian);
            checkDataDate(newJadwalUjian);
            checkFormatAndRangeDate(newJadwalUjian);

            //cancel ujian lama
            Result<Domain.Ujian.Ujian> prevUjian = Domain.Ujian.Ujian.Update(existingUjian)
                         .ChangeStatus("cancel")
                         .Build();

            if (prevUjian.IsFailure)
            {
                return Result.Failure<Guid>(prevUjian.Error);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //insert ujian baru
            var newUjian = Domain.Ujian.Ujian.Create(request.NoReg, int.Parse(newJadwalUjian?.Id ?? "0"));
            if (newUjian.IsFailure)
            {
                return Result.Failure<Guid>(newUjian.Error);
            }
            ujianRepository.Insert(newUjian.Value);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //insert list pertanyaan
            Domain.Ujian.Ujian? currentUjian = await ujianRepository.GetAsync(newUjian.Value.Uuid, cancellationToken);
            if (currentUjian.Id == null)
            {
                return Result.Failure<Guid>(UjianErrors.NotFoundReference());
            }

            List<TemplatePertanyaanResponse> listMasterPertanyaan = await templatePertanyaanApi.GetAllTemplatePertanyaanByBankSoal(newJadwalUjian.IdBankSoal);
            IEnumerable<Cbt> listPertanyaan = listMasterPertanyaan.Select(item =>
                Cbt.Create(
                    currentUjian?.Id ?? 0,
                    int.Parse(item.Id)
                ).Value
            );
            await cbtRepository.InsertAsync(listPertanyaan);

            await unitOfWork.SaveChangesAsync(cancellationToken);







            string oldKey = "counter_" + request.prevIdJadwalUjian.ToString();
            bool checkOldKey = await counterRepository.KeyExistsAsync(key);
            int prevCounter = await counterRepository.GetCounterAsync(oldKey);
            if (checkOldKey && prevCounter > 0)
            {
                await counterRepository.DecrementCounterAsync(oldKey, null);
            }

            if (checkNewKey) {
                var timeToExpire = GetTimeToExpire(newJadwalUjian.Tanggal, newJadwalUjian.JamMulai);
                string newKey = $"counter_{request.newIdJadwalUjian}";
                await counterRepository.ResetCounterAsync(newKey, 0, timeToExpire);
            }

            return Result.Success(newUjian.Value.Uuid);
        }

        private Result? checkData(Guid id, JadwalUjianResponse? jadwalUjian)
        {
            if (jadwalUjian is null)
            {
                return Result.Failure<Guid>(Domain.JadwalUjian.JadwalUjianErrors.NotFound(id));
            }

            return null;
        }
        private Result? checkDataDate(JadwalUjianResponse? jadwalUjian)
        {
            if (string.IsNullOrWhiteSpace(jadwalUjian.Tanggal) ||
                    string.IsNullOrWhiteSpace(jadwalUjian.JamMulai) ||
                    string.IsNullOrWhiteSpace(jadwalUjian.JamAkhir))
            {
                return Result.Failure<Guid>(Domain.JadwalUjian.JadwalUjianErrors.EmptyDataScheduleFormat());
            }

            return null;
        }
        private Result? checkFormatAndRangeDate(JadwalUjianResponse? jadwalUjian)
        {
            if (!DateTime.TryParseExact(jadwalUjian.Tanggal + " " + jadwalUjian.JamMulai, "yyyy-MM-dd HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var mulai))
            {
                return Result.Failure<Guid>(UjianErrors.InvalidScheduleFormat("start"));
            }

            if (!DateTime.TryParseExact(jadwalUjian.Tanggal + " " + jadwalUjian.JamAkhir, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var akhir))
            {
                return Result.Failure<Guid>(UjianErrors.InvalidScheduleFormat("end"));
            }

            var sekarang = DateTime.UtcNow;
            if (mulai > akhir)
            {
                return Result.Failure<Guid>(UjianErrors.InvalidRangeDateTime());
            }

            if (sekarang >= mulai && sekarang <= akhir)
            {
                return Result.Failure<Guid>(UjianErrors.OutRange(mulai.ToString("yyyy-MM-dd HH:mm"), akhir.ToString("yyyy-MM-dd HH:mm")));
            }

            return null;
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
