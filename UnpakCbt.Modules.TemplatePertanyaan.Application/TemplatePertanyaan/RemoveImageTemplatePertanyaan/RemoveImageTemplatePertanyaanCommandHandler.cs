using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplatePertanyaan.Application.Abstractions.Data;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.UpdateTemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.RemoveImageTemplatePertanyaan
{
    internal sealed class RemoveImageTemplatePertanyaanCommandHandler(
    ITemplatePertanyaanRepository templatePertanyaanRepository,
    IUnitOfWork unitOfWork,
    ILogger<RemoveImageTemplatePertanyaanCommand> logger)
    : ICommandHandler<RemoveImageTemplatePertanyaanCommand>
    {
        public async Task<Result> Handle(RemoveImageTemplatePertanyaanCommand request, CancellationToken cancellationToken)
        {
            Domain.TemplatePertanyaan.TemplatePertanyaan? existingTemplatePertanyaan = await templatePertanyaanRepository.GetAsync(request.Uuid, cancellationToken);

            if (existingTemplatePertanyaan is null)
            {
                logger.LogError($"TemplatePertanyan dengan referensi uuid {request.Uuid} tidak ditemukan");
                return Result.Failure(TemplatePertanyaanErrors.NotFound(request.Uuid));
            }

            Result<Domain.TemplatePertanyaan.TemplatePertanyaan> templatePertanyaan1 = Domain.TemplatePertanyaan.TemplatePertanyaan.Update(existingTemplatePertanyaan!)
                         .ChangePertanyaanImg(null)
                         .Build();

            if (templatePertanyaan1.IsFailure)
            {
                logger.LogError("domain bisnis TemplatePertanyan tidak sesuai aturan");
                return Result.Failure<Guid>(templatePertanyaan1.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("berhasil remove image TemplatePertanyaan dengan referensi Uuid {uuid}",request.Uuid);

            return Result.Success();
        }
    }
}
