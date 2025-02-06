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
using UnpakCbt.Modules.TemplatePertanyaan.PublicApi;
using UnpakCbt.Modules.TemplateJawaban.PublicApi;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.UpdateCbt
{
    internal sealed class UpdateCbtCommandHandler(
    IUjianRepository ujianRepository,
    ICbtRepository cbtRepository,
    IUnitOfWork unitOfWork,
    IJadwalUjianApi jadwalUjianApi,
    ITemplateJawabanApi templateJawabanApi)
    : ICommandHandler<UpdateCbtCommand>
    {
        public async Task<Result> Handle(UpdateCbtCommand request, CancellationToken cancellationToken)
        {
            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetAsync(request.Uuid, cancellationToken);

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

            //[PR]
            /*if (sekarang < mulai && sekarang > akhir)
            {
                return Result.Failure<Guid>(UjianErrors.OutRangeExam(mulai.ToString("yyyy-MM-dd HH:mm"), akhir.ToString("yyyy-MM-dd HH:mm")));
            }
            if (existingUjian.NoReg != request.NoReg)
            {
                return Result.Failure<Guid>(UjianErrors.IncorrectReferenceNoReg(request.NoReg, existingUjian?.NoReg ?? "-"));
            }

            if (existingUjian?.Status == "active")
            {
                return Result.Failure<Guid>(UjianErrors.ScheduleExamNoStartExam());
            }
            if (existingUjian?.Status == "done")
            {
                return Result.Failure<Guid>(UjianErrors.ScheduleExamDoneExam());
            }
            if (existingUjian?.Status == "cancel")
            {
                return Result.Failure<Guid>(UjianErrors.ScheduleExamCancelExam());
            }*/

            int? JawabanBenar = null;
            if (request?.IdJawabanBenar != null)
            {
                TemplateJawabanResponse? templateJawaban = await templateJawabanApi.GetAsync(request.IdJawabanBenar, cancellationToken);

                if (templateJawaban is null)
                {
                    return Result.Failure<Guid>(TemplateJawabanErrors.NotFound(request?.IdJawabanBenar ?? Guid.Empty));
                }

                JawabanBenar = int.Parse(templateJawaban.Id);
            }
            if (JawabanBenar==null || JawabanBenar <=0) {
                return Result.Failure<Guid>(UjianErrors.IdJawabanBenarNotFound(request?.IdJawabanBenar??Guid.Empty));
            }

            Cbt existingCbt = await cbtRepository.GetAsync(request.Uuid);
            Result<Cbt> currentCbt = Cbt.Update(existingCbt)
                         .ChangeJawabanBenar(JawabanBenar)
                         .Build();

            await unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}
