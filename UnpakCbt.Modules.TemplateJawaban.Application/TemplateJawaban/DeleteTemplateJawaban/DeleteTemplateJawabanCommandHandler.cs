using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplateJawaban.Application.Abstractions.Data;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.CreateTemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.DeleteTemplateJawaban
{
    internal sealed class DeleteTemplateJawabanCommandHandler(
    ITemplateJawabanRepository templateJawabanRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteTemplateJawabanCommand> logger)
    : ICommandHandler<DeleteTemplateJawabanCommand>
    {
        public async Task<Result> Handle(DeleteTemplateJawabanCommand request, CancellationToken cancellationToken)
        {
            Domain.TemplateJawaban.TemplateJawaban? existingTemplateJawaban = await templateJawabanRepository.GetAsync(request.uuid, cancellationToken);

            if (existingTemplateJawaban is null)
            {
                logger.LogError($"TemplateJawaban dengan referensi Uuid {request.uuid} tidak ditemukan");
                return Result.Failure(TemplateJawabanErrors.NotFound(request.uuid));
            }

            string? filePath = null;
            if (!string.IsNullOrEmpty(existingTemplateJawaban.JawabanImg))
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads/jawaban_img");
                filePath = Path.Combine(uploadsFolder, existingTemplateJawaban.JawabanImg);
                logger.LogInformation("setup path file {filePath}",filePath);
            }
            
            await templateJawabanRepository.DeleteAsync(existingTemplateJawaban);
            logger.LogInformation("berhasil hapus TemplateJawaban dengan referensi Uuid {uuid}",request.uuid);

            if (filePath != null && File.Exists(filePath))
            {
                File.Delete(filePath);
                logger.LogInformation("berhasil hapus file {filePath}",filePath);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
