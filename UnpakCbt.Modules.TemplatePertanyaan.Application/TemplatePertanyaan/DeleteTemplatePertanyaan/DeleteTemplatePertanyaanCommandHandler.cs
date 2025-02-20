using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplatePertanyaan.Application.Abstractions.Data;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.DeleteTemplatePertanyaan
{
    internal sealed class DeleteTemplatePertanyaanCommandHandler(
    ITemplatePertanyaanRepository templatePertanyaanRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteTemplatePertanyaanCommand> logger)
    : ICommandHandler<DeleteTemplatePertanyaanCommand>
    {
        public async Task<Result> Handle(DeleteTemplatePertanyaanCommand request, CancellationToken cancellationToken)
        {
            Domain.TemplatePertanyaan.TemplatePertanyaan? existingTemplatePertanyaan = await templatePertanyaanRepository.GetAsync(request.uuid, cancellationToken);

            if (existingTemplatePertanyaan is null)
            {
                logger.LogError($"TemplatePertanyaan dengan referensi Uuid {request.uuid} tidak ditemukan");
                return Result.Failure(TemplatePertanyaanErrors.NotFound(request.uuid));
            }

            string? filePath = null;
            if (!string.IsNullOrEmpty(existingTemplatePertanyaan.PertanyaanImg))
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads/pertanyaan_img");
                filePath = Path.Combine(uploadsFolder, existingTemplatePertanyaan.PertanyaanImg);
                logger.LogInformation($"setup path {filePath}");
            }
            
            await templatePertanyaanRepository.DeleteAsync(existingTemplatePertanyaan);
            
            if (filePath != null && File.Exists(filePath))
            {
                File.Delete(filePath);
                logger.LogInformation($"berhasil hapus file {filePath}");
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"berhasil hapus TemplatePertanyaan dengan referensi uuid {request.uuid}");

            return Result.Success();
        }
    }
}
