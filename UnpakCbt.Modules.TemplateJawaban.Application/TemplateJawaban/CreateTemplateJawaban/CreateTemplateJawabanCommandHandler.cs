using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplateJawaban.Application.Abstractions.Data;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.PublicApi;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.CreateTemplateJawaban
{
    internal sealed class CreateTemplateJawabanCommandHandler(
    ITemplateJawabanRepository templateJawabanRepository,
    IUnitOfWork unitOfWork,
    ITemplatePertanyaanApi templatePertanyaanApi,
    ILogger<CreateTemplateJawabanCommand> logger)
    : ICommandHandler<CreateTemplateJawabanCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateTemplateJawabanCommand request, CancellationToken cancellationToken)
        {
            TemplatePertanyaanResponse? templatePertanyaan = await templatePertanyaanApi.GetAsync(request.IdTemplateSoal, cancellationToken);

            if (templatePertanyaan is null)
            {
                logger.LogError($"TemplatePertanyaan dengan referensi Uuid {request.IdTemplateSoal} tidak ditemukan");
                return Result.Failure<Guid>(TemplatePertanyaanErrors.NotFound(request.IdTemplateSoal));
            }

            Result<Domain.TemplateJawaban.TemplateJawaban> result = Domain.TemplateJawaban.TemplateJawaban.Create(
                int.Parse(templatePertanyaan.Id), //int.Parse(bankSoal.Value.Id)
                request.JawabanText,
                request.JawabanImg
            );

            if (result.IsFailure)
            {
                logger.LogError("domain bisnis TemplateJawaban tidak sesuai aturan");
                return Result.Failure<Guid>(result.Error);
            }

            templateJawabanRepository.Insert(result.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("berhasil simpan TemplateJawaban dengan hasil uuid {uuid}", result.Value.Uuid);

            return result.Value.Uuid;
        }
    }
}
