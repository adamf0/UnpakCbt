using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Application.Abstractions.Data;
using UnpakCbt.Modules.TemplatePertanyaan.PublicApi;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;
using Microsoft.Extensions.Logging;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.DeleteTemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.UpdateTemplateJawaban
{
    internal sealed class UpdateTemplateJawabanCommandHandler(
    ITemplateJawabanRepository templateJawabanRepository,
    IUnitOfWork unitOfWork,
    ITemplatePertanyaanApi templatePertanyaanApi,
    ILogger<UpdateTemplateJawabanCommand> logger)
    : ICommandHandler<UpdateTemplateJawabanCommand>
    {
        public async Task<Result> Handle(UpdateTemplateJawabanCommand request, CancellationToken cancellationToken)
        {
            TemplatePertanyaanResponse? templatePertanyaan = await templatePertanyaanApi.GetAsync(request.IdTemplateSoal, cancellationToken);

            if (templatePertanyaan is null)
            {
                logger.LogError($"TemplatePertanyaan dengan referensi Uuid {request.IdTemplateSoal} tidak ditemukan");
                return Result.Failure<Guid>(TemplatePertanyaanErrors.NotFound(request.IdTemplateSoal));
            }

            Domain.TemplateJawaban.TemplateJawaban? existingTemplateJawaban = await templateJawabanRepository.GetAsync(request.Uuid, cancellationToken);

            if (existingTemplateJawaban is null)
            {
                logger.LogError($"TemplateJawaban dengan referensi Uuid {request.Uuid} tidak ditemukan");
                return Result.Failure(TemplateJawabanErrors.NotFound(request.Uuid));
            }

            Result<Domain.TemplateJawaban.TemplateJawaban> asset = Domain.TemplateJawaban.TemplateJawaban.Update(existingTemplateJawaban!)
                         .ChangeTemplateSoal(int.Parse(templatePertanyaan.Id)) //int.Parse(bankSoal.Value.Id)
                         .ChangeJawabanText(request.JawabanText)
                         .ChangeJawabanImg(request.JawabanImg)
                         .Build();

            if (asset.IsFailure)
            {
                logger.LogError("domain bisnis TemplateJawaban tidak sesuai aturan");
                return Result.Failure<Guid>(asset.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation("berhasil update TemplateJawaban dengan referensi uuid {uuid}",request.Uuid);

            return Result.Success();
        }
    }
}
