using System.Globalization;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.PublicApi;
using UnpakCbt.Modules.TemplatePertanyaan.PublicApi;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.Ujian.Domain.Cbt;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.UpdateUjian
{
    internal sealed class UpdateUjianCommandHandler(
    IUjianRepository ujianRepository,
    ICbtRepository cbtRepository,
    IUnitOfWork unitOfWork,
    IJadwalUjianApi jadwalUjianApi,
    ITemplatePertanyaanApi templatePertanyaanApi)
    : ICommandHandler<UpdateUjianCommand>
    {
        public async Task<Result> Handle(UpdateUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetAsync(request.Uuid, cancellationToken);

            if (existingUjian is null)
            {
                Result.Failure(UjianErrors.NotFound(request.Uuid));
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

            JadwalUjianResponse? jadwalUjian = await jadwalUjianApi.GetAsync(request.IdJadwalUjian, cancellationToken);
            if (jadwalUjian is null)
            {
                return Result.Failure<Guid>(Domain.JadwalUjian.JadwalUjianErrors.NotFound(request.IdJadwalUjian));
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

            //hapus list pertanyaan
            await cbtRepository.DeleteAsync(existingUjian?.Id??0);
            await unitOfWork.SaveChangesAsync(cancellationToken);


            //update ujian
            Result<Domain.Ujian.Ujian> asset = Domain.Ujian.Ujian.Update(existingUjian!)
                         .ChangeNoReg(request.NoReg)
                         .ChangeJadwalUjian(int.Parse(jadwalUjian.Id))
                         .Build();

            if (asset.IsFailure)
            {
                return Result.Failure<Guid>(asset.Error);
            }
            await unitOfWork.SaveChangesAsync(cancellationToken);


            //insert list pertanyaan
            if (existingUjian?.Id == null) {
                return Result.Failure<Guid>(UjianErrors.NotFoundReference());
            }

            List<TemplatePertanyaanResponse> listMasterPertanyaan = await templatePertanyaanApi.GetAllTemplatePertanyaanByBankSoal(jadwalUjian.IdBankSoal);
            IEnumerable<Domain.Cbt.Cbt> listPertanyaan = listMasterPertanyaan.Select(item =>
                Domain.Cbt.Cbt.Create(
                    existingUjian.Id ?? 0,
                    int.Parse(item.Id)
                ).Value
            );
            await cbtRepository.InsertAsync(listPertanyaan);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
