﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Domain.Ujian;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.DoneUjian
{
    internal sealed class DoneUjianCommandHandler(
    IUjianRepository ujianRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DoneUjianCommand>
    {
        public async Task<Result> Handle(DoneUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetAsync(request.uuid, cancellationToken);
            if (existingUjian is null)
            {
                Result.Failure(UjianErrors.NotFound(request.uuid));
            }
            if (existingUjian?.NoReg != request.NoReg)
            {
                return Result.Failure<Guid>(UjianErrors.IncorrectReferenceNoReg(request.NoReg, existingUjian?.NoReg??"-"));
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
            }

            return Result.Success();
        }
    }
}
