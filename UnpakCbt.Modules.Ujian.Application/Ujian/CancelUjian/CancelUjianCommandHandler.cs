using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Domain.Ujian;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.JadwalUjian.PublicApi;
using System.Globalization;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;
using MediatR;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.CancelUjian
{
    internal sealed class CancelUjianCommandHandler(
    Domain.Ujian.ICounterRepository counterRepository,
    IUjianRepository ujianRepository,
    IUnitOfWork unitOfWork,
    IJadwalUjianApi jadwalUjianApi)
    : ICommandHandler<CancelUjianCommand>
    {
        public async Task<Result> Handle(CancelUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetAsync(request.uuid, cancellationToken);
            if (existingUjian is null)
            {
                Result.Failure(UjianErrors.NotFound(request.uuid));
            }

            JadwalUjianResponse? jadwalUjian = await jadwalUjianApi.GetByIdAsync(existingUjian?.IdJadwalUjian, cancellationToken);
            if (jadwalUjian is null)
            {
                return Result.Failure<Guid>(UjianErrors.ScheduleExamNotFound(Guid.Parse(jadwalUjian!.Uuid)));
            }

            if (existingUjian?.Status == "cancel") {
                return Result.Failure<Guid>(UjianErrors.ScheduleExamDoneCancel());
            }

            Result<Domain.Ujian.Ujian> prevUjian = Domain.Ujian.Ujian.Update(existingUjian!)
                         .ChangeStatus("cancel")
                         .Build();

            if (prevUjian.IsFailure)
            {
                return Result.Failure<Guid>(prevUjian.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            string oldKey = "counter_" + jadwalUjian.Uuid;
            await counterRepository.DecrementCounterAsync(oldKey, null);

            return Result.Success();
        }

        private Result? checkData(Guid id, JadwalUjianResponse? jadwalUjian)
        {
            if (jadwalUjian is null)
            {
                return Result.Failure<Guid>(JadwalUjianErrors.NotFound(id));
            }

            return null;
        }
        private Result? checkDataDate(JadwalUjianResponse? jadwalUjian)
        {
            if (string.IsNullOrWhiteSpace(jadwalUjian.Tanggal) ||
                    string.IsNullOrWhiteSpace(jadwalUjian.JamMulai) ||
                    string.IsNullOrWhiteSpace(jadwalUjian.JamAkhir))
            {
                return Result.Failure<Guid>(JadwalUjianErrors.EmptyDataScheduleFormat());
            }

            return null;
        }
        private Result? checkFormatAndRangeDate(JadwalUjianResponse? jadwalUjian)
        {
            if (!DateTime.TryParseExact(jadwalUjian.Tanggal + " " + jadwalUjian.JamMulai, "yyyy-MM-dd HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var mulai))
            {
                return Result.Failure<Guid>(UjianErrors.InvalidScheduleFormat("start date"));
            }

            if (!DateTime.TryParseExact(jadwalUjian.Tanggal + " " + jadwalUjian.JamAkhir, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var akhir))
            {
                return Result.Failure<Guid>(UjianErrors.InvalidScheduleFormat("end date"));
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
