﻿using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.Ujian.Domain.Ujian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.StartUjian
{
    internal sealed class StartUjianCommandHandler(
    IUjianRepository ujianRepository,
    IUnitOfWork unitOfWork,
    ILogger<StartUjianCommand> logger)
    : ICommandHandler<StartUjianCommand>
    {
        public async Task<Result> Handle(StartUjianCommand request, CancellationToken cancellationToken)
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
                logger.LogError($"Data ujian {request.NoReg} sudah start");
                return Result.Failure<Guid>(UjianErrors.ScheduleExamStartExam());
            }
            if (existingUjian?.Status == "active")
            {
                Result<Domain.Ujian.Ujian> prevUjian = Domain.Ujian.Ujian.Update(existingUjian!)
                         .ChangeStatus("start")
                         .Build();

                if (prevUjian.IsFailure)
                {
                    return Result.Failure<Guid>(prevUjian.Error);
                }
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return Result.Success();
        }
    }
}
