using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Application.Abstractions.Data;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.DeleteJadwalUjian
{
    internal sealed class DeleteJadwalUjianCommandHandler(
    IJadwalUjianRepository jadwalUjianRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteJadwalUjianCommand>
    {
        public async Task<Result> Handle(DeleteJadwalUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.JadwalUjian.JadwalUjian? existingJadwalUjian = await jadwalUjianRepository.GetAsync(request.uuid, cancellationToken);

            if (existingJadwalUjian is null)
            {
                return Result.Failure(JadwalUjianErrors.NotFound(request.uuid));
            }

            await jadwalUjianRepository.DeleteAsync(existingJadwalUjian!);
            //event update change table position asset, order desc + select first

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
