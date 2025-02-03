using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplatePertanyaan.Application.Abstractions.Data;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.DeleteTemplatePertanyaan
{
    internal sealed class DeleteTemplatePertanyaanCommandHandler(
    ITemplatePertanyaanRepository templatePertanyaanRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteTemplatePertanyaanCommand>
    {
        public async Task<Result> Handle(DeleteTemplatePertanyaanCommand request, CancellationToken cancellationToken)
        {
            Domain.TemplatePertanyaan.TemplatePertanyaan? existingTemplatePertanyaan = await templatePertanyaanRepository.GetAsync(request.uuid, cancellationToken);

            if (existingTemplatePertanyaan is null)
            {
                return Result.Failure(TemplatePertanyaanErrors.NotFound(request.uuid));
            }

            await templatePertanyaanRepository.DeleteAsync(existingTemplatePertanyaan!);
            //event update change table position asset, order desc + select first

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
