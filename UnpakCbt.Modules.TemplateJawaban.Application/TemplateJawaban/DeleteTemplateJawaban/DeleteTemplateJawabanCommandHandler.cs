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

            string? filePath = null;
            if (!string.IsNullOrEmpty(existingTemplateJawaban.JawabanImg))
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "jawaban_img");
                filePath = Path.Combine(uploadsFolder, existingTemplateJawaban.JawabanImg);

            }
            
            await templateJawabanRepository.DeleteAsync(existingTemplateJawaban);

            if (filePath != null && File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
