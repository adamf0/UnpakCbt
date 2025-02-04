using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Domain.Ujian;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.DeleteUjian
{
    internal sealed class DeleteUjianCommandHandler(
    IUjianRepository ujianRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteUjianCommand>
    {
        public async Task<Result> Handle(DeleteUjianCommand request, CancellationToken cancellationToken)
        {
            Domain.Ujian.Ujian? existingUjian = await ujianRepository.GetAsync(request.uuid, cancellationToken);

            if (existingUjian is null)
            {
                return Result.Failure(UjianErrors.NotFound(request.uuid));
            }

            await ujianRepository.DeleteAsync(existingUjian!);
            //event update change table position asset, order desc + select first

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
