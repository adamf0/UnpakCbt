﻿using System;
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

namespace UnpakCbt.Modules.Ujian.Application.Ujian.RescheduleUjian
{
    internal sealed class RescheduleUjianCommandHandler( // [Test]
    Domain.Ujian.ICounterRepository counterRepository,
    IUjianRepository ujianRepository,
    IUnitOfWork unitOfWork,
    IJadwalUjianApi jadwalUjianApi)
    : ICommandHandler<RescheduleUjianCommand,Guid>
    {
        public async Task<Result<Guid>> Handle(RescheduleUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetAsync(request.prevIdJadwalUjian, cancellationToken); //ganti bukan id tapi uuid
            if (existingUjian is null)
            {
                Result.Failure(UjianErrors.NotFound(request.prevIdJadwalUjian));
            }

            JadwalUjianResponse? jadwalUjian = await jadwalUjianApi.GetAsync(request.newIdJadwalUjian, cancellationToken);
            checkData(request.newIdJadwalUjian, jadwalUjian);
            checkDataDate(jadwalUjian);
            checkFormatAndRangeDate(jadwalUjian);

            Result<Domain.Ujian.Ujian> prevUjian = Domain.Ujian.Ujian.Update(existingUjian!)
                         .ChangeStatus("cancel")
                         .Build();

            if (prevUjian.IsFailure)
            {
                return Result.Failure<Guid>(prevUjian.Error);
            }

            string oldKey = "counter_" + request.prevIdJadwalUjian.ToString();
            await counterRepository.DecrementCounterAsync(oldKey, null);

            var newUjian = Domain.Ujian.Ujian.Create(request.NoReg, int.Parse(jadwalUjian.Id));
            if (newUjian.IsFailure)
            {
                return Result.Failure<Guid>(newUjian.Error);
            }

            var timeToExpire = GetTimeToExpire(jadwalUjian.Tanggal, jadwalUjian.JamMulai);
            string newKey = $"counter_{request.newIdJadwalUjian}";
            await counterRepository.ResetCounterAsync(newKey, 0, timeToExpire);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(newUjian.Value.Uuid);
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
