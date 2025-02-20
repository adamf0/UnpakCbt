using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Application.Abstractions.Data;
using Microsoft.Extensions.Logging;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.CreateJadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.DeleteJadwalUjian
{
    internal sealed class DeleteJadwalUjianCommandHandler(
    ICounterRepository counterRepository,
    IJadwalUjianRepository jadwalUjianRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteJadwalUjianCommand> logger)
    : ICommandHandler<DeleteJadwalUjianCommand>
    {
        public async Task<Result> Handle(DeleteJadwalUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.JadwalUjian.JadwalUjian? existingJadwalUjian = await jadwalUjianRepository.GetAsync(request.uuid, cancellationToken);

            if (existingJadwalUjian is null)
            {
                logger.LogError($"JadwalUjian dengan referensi Uuid {request.uuid} tidak ditemukan");
                return Result.Failure(JadwalUjianErrors.NotFound(request.uuid));
            }

            await jadwalUjianRepository.DeleteAsync(existingJadwalUjian!);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"berhasil hapus JadwalUjian dengan referensi Uuid {request.uuid}");

            string key = "counter_" + request.uuid.ToString();
            await counterRepository.DeleteKeyAsync(key);
            logger.LogInformation($"berhasil hapus {key}");

            return Result.Success();
        }
    }
}
