using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.Ujian.Application.StreamHub;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.DoneUjian
{
    internal sealed class DoneUjianCommandHandler(
    IHubContext<JadwalUjianHub> _hubContext,
    IUjianRepository ujianRepository,
    IUnitOfWork unitOfWork,
    ILogger<DoneUjianCommand> logger)
    : ICommandHandler<DoneUjianCommand>
    {
        public async Task<Result> Handle(DoneUjianCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Received command with parameters: {@Request}", request);

            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetAsync(request.uuid, cancellationToken);
            logger.LogInformation("existingUjian: {@existingUjian}", existingUjian);
            if (existingUjian is null)
            {
                logger.LogError($"Ujian dengan referensi Uuid {request.uuid} tidak ditemukan");
                return Result.Failure(UjianErrors.NotFound(request.uuid));
            }
            if (existingUjian?.NoReg != request.NoReg)
            {
                logger.LogError($"Ujian dengan referensi Uuid {request.uuid} tidak untuk NoReg {request.NoReg}");
                return Result.Failure<Guid>(UjianErrors.IncorrectReferenceNoReg(request.uuid, request.NoReg));
            }

            if (existingUjian?.Status == "active")
            {
                logger.LogError($"Data ujian {request.NoReg} sudah active");
                return Result.Failure<Guid>(UjianErrors.ScheduleExamNotStartedExam());
            }
            if (existingUjian?.Status == "cancel")
            {
                logger.LogError($"Data ujian {request.NoReg} sudah cancel");
                return Result.Failure<Guid>(UjianErrors.ScheduleExamCancelExam());
            }
            if (existingUjian?.Status == "done")
            {
                logger.LogError($"Data ujian {request.NoReg} sudah done");
                return Result.Failure<Guid>(UjianErrors.ScheduleExamDoneExam());
            }

            if (existingUjian?.Status == "start")
            {
                Result<Domain.Ujian.Ujian> prevUjian = Domain.Ujian.Ujian.Update(existingUjian!)
                         .ChangeStatus("done")
                         .Build();

                if (prevUjian.IsFailure)
                {
                    return Result.Failure<Guid>(prevUjian.Error);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);
                try
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveJadwalUjianUpdate", new UjianDetailResponse
                    {
                        Uuid = request.uuid.ToString(),
                        NoReg = request.NoReg,
                        Status = "done"
                    });
                }
                catch (HubException hubEx)
                {
                    logger.LogError($"SignalR error: {hubEx.Message}");
                }
                catch (TaskCanceledException taskEx)
                {
                    logger.LogError($"SignalR request dibatalkan: {taskEx.Message}");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Unexpected error: {ex.Message}");
                }

            }

            return Result.Success();
        }
    }
}
