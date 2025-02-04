using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Domain.Ujian;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.JadwalUjian.PublicApi;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;
using System.Reflection.Metadata.Ecma335;
using MediatR;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.CreateUjian
{
    internal sealed class CreateUjianCommandHandler(
        Domain.Ujian.ICounterRepository counterRepository,
        IUjianRepository ujianRepository,
        IUnitOfWork unitOfWork,
        IJadwalUjianApi jadwalUjianApi)
        : ICommandHandler<CreateUjianCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateUjianCommand request, CancellationToken cancellationToken)
        {
            JadwalUjianResponse? jadwalUjian = await jadwalUjianApi.GetAsync(request.IdJadwalUjian, cancellationToken);
            checkData(jadwalUjian, request.IdJadwalUjian);
            checkDataDate(jadwalUjian);
            checkFormatAndRangeDate(jadwalUjian);
            //check unique data [PR]

            var result = Domain.Ujian.Ujian.Create(request.NoReg, int.Parse(jadwalUjian.Id));
            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }

            string key = "counter_" + request.IdJadwalUjian.ToString();

            if (jadwalUjian.Kuota > 0) {    
                int postCounter = await counterRepository.GetCounterAsync(key) + 1;
                if (postCounter > jadwalUjian.Kuota)
                {
                    return Result.Failure<Guid>(UjianErrors.QuotaExhausted(postCounter.ToString(), jadwalUjian.Kuota.ToString()));
                }
            }
            await counterRepository.IncrementCounterAsync(key, null);

            ujianRepository.Insert(result.Value);
            //tahap 1: insert ujian_cbt manual
            await unitOfWork.SaveChangesAsync(cancellationToken);
            //tahap 2: insert ujian_cbt background job

            return result.Value.Uuid;
        }

        private Result? checkData(JadwalUjianResponse? jadwalUjian, Guid IdJadwalUjian) {
            if (jadwalUjian is null)
            {
                return Result.Failure<Guid>(JadwalUjianErrors.NotFound(IdJadwalUjian));
            }

            return null;
        }
        private Result? checkDataDate(JadwalUjianResponse? jadwalUjian) {
            if (string.IsNullOrWhiteSpace(jadwalUjian.Tanggal) ||
                    string.IsNullOrWhiteSpace(jadwalUjian.JamMulai) ||
                    string.IsNullOrWhiteSpace(jadwalUjian.JamAkhir))
            {
                return Result.Failure<Guid>(UjianErrors.EmptyDataScheduleFormat());
            }

            return null;
        }
        private Result? checkFormatAndRangeDate(JadwalUjianResponse? jadwalUjian) {
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
    }
}
