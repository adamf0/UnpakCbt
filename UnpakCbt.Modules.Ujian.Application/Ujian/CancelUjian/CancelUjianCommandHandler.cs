using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.PublicApi;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.CancelUjian
{
    internal sealed class CancelUjianCommandHandler(
    Domain.Ujian.ICounterRepository counterRepository,
    IUjianRepository ujianRepository,
    IUnitOfWork unitOfWork,
    IJadwalUjianApi jadwalUjianApi,
    ILogger<CancelUjianCommand> logger)
    : ICommandHandler<CancelUjianCommand>
    {
        public async Task<Result> Handle(CancelUjianCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Received command with parameters: {@Request}", request);

            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetAsync(request.uuid, cancellationToken);
            logger.LogInformation("existingUjian: {@existingUjian}", existingUjian);
            if (existingUjian is null)
            {
                logger.LogError($"Ujian dengan referensi Uuid {request.uuid} tidak ditemukan");
                return Result.Failure(UjianErrors.NotFound(request.uuid));
            }
            if (existingUjian?.NoReg != request.noReg)
            {
                logger.LogError($"Ujian dengan referensi Uuid {request.uuid} tidak untuk NoReg {request.noReg}");
                return Result.Failure<Guid>(UjianErrors.IncorrectReferenceNoReg(request.uuid, request.noReg));
            }

            JadwalUjianResponse? jadwalUjian = await jadwalUjianApi.GetByIdAsync(existingUjian?.IdJadwalUjian, cancellationToken);
            logger.LogInformation("jadwalUjian: {@jadwalUjian}", jadwalUjian);

            if (jadwalUjian is null)
            {
                logger.LogError($"JadwalUjian dengan referensi id {existingUjian?.IdJadwalUjian} tidak ditemukan");
                return Result.Failure<Guid>(UjianErrors.ScheduleExamNotFound(Guid.Parse(jadwalUjian!.Uuid)));
            }

            if (existingUjian?.Status == "cancel") {
                logger.LogError($"status Ujian dengan referensi Uuid {request?.uuid} sudah cancel");
                return Result.Failure<Guid>(UjianErrors.ScheduleExamCancelExam());
            }
            if (existingUjian?.Status == "done") {
                logger.LogError($"status Ujian dengan referensi Uuid {request?.uuid} sudah done");
                return Result.Failure<Guid>(UjianErrors.ScheduleExamDoneExam());
            }
            if (existingUjian?.Status == "start") {
                logger.LogError($"status Ujian dengan referensi Uuid {request?.uuid} sudah start");
                return Result.Failure<Guid>(UjianErrors.ScheduleExamStartExam());
            }

            Result<Domain.Ujian.Ujian> prevUjian = Domain.Ujian.Ujian.Update(existingUjian!)
                         .ChangeStatus("cancel")
                         .Build();

            if (prevUjian.IsFailure)
            {
                logger.LogError("domain bisnis Ujian tidak sesuai aturan");
                return Result.Failure<Guid>(prevUjian.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("berhasil cancel Ujian dengan referensi Uuid {uuid}",request.uuid);

            string oldKey = "counter_" + jadwalUjian.Uuid;
            int prevCounter = await counterRepository.GetCounterAsync(oldKey);
            bool checkKey = await counterRepository.KeyExistsAsync(oldKey);
            if (checkKey && prevCounter > 0)
            {
                await counterRepository.DecrementCounterAsync(oldKey, null);
                logger.LogInformation("berhasil decrement key {oldKey}",oldKey);
            }

            return Result.Success();
        }
    }
}
