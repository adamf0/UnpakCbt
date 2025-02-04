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

namespace UnpakCbt.Modules.Ujian.Application.Ujian.UpdateUjian
{
    internal sealed class UpdateUjianCommandHandler(
    IUjianRepository ujianRepository,
    IUnitOfWork unitOfWork,
    IJadwalUjianApi jadwalUjianApi)
    : ICommandHandler<UpdateUjianCommand>
    {
        public async Task<Result> Handle(UpdateUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetAsync(request.Uuid, cancellationToken);

            if (existingUjian is null)
            {
                Result.Failure(UjianErrors.NotFound(request.Uuid));
            }

            JadwalUjianResponse? jadwalUjian = await jadwalUjianApi.GetAsync(request.IdJadwalUjian, cancellationToken);
            if (jadwalUjian is null)
            {
                return Result.Failure<Guid>(JadwalUjianErrors.NotFound(request.IdJadwalUjian));
            }

            if (string.IsNullOrWhiteSpace(jadwalUjian.Tanggal) ||
                string.IsNullOrWhiteSpace(jadwalUjian.JamMulai) ||
                string.IsNullOrWhiteSpace(jadwalUjian.JamAkhir))
            {
                return Result.Failure<Guid>(UjianErrors.EmptyDataScheduleFormat());
            }

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

            Result<Domain.Ujian.Ujian> asset = Domain.Ujian.Ujian.Update(existingUjian!)
                         .ChangeNoReg(request.NoReg)
                         .ChangeJadwalUjian(int.Parse(jadwalUjian.Id))
                         .ChangeStatus(request.Status)
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
