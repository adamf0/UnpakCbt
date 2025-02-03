using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplateJawaban.Application.Abstractions.Data;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.DeleteTemplateJawaban
{
    internal sealed class DeleteTemplateJawabanCommandHandler(
    ITemplateJawabanRepository templateJawabanRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteTemplateJawabanCommand>
    {
        public async Task<Result> Handle(DeleteTemplateJawabanCommand request, CancellationToken cancellationToken)
        {
            Domain.TemplateJawaban.TemplateJawaban? existingTemplateJawaban = await templateJawabanRepository.GetAsync(request.uuid, cancellationToken);

            if (existingTemplateJawaban is null)
            {
                return Result.Failure(TemplateJawabanErrors.NotFound(request.uuid));
            }

            await templateJawabanRepository.DeleteAsync(existingTemplateJawaban!);
            //event update change table position asset, order desc + select first

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
