using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Application.Abstractions.Data;
using UnpakCbt.Modules.TemplatePertanyaan.PublicApi;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.UpdateTemplateJawaban
{
    internal sealed class UpdateTemplateJawabanCommandHandler(
    ITemplateJawabanRepository templateJawabanRepository,
    IUnitOfWork unitOfWork,
    ITemplatePertanyaanApi templatePertanyaanApi)
    : ICommandHandler<UpdateTemplateJawabanCommand>
    {
        public async Task<Result> Handle(UpdateTemplateJawabanCommand request, CancellationToken cancellationToken)
        {
            TemplatePertanyaanResponse? templatePertanyaan = await templatePertanyaanApi.GetAsync(request.IdTemplateSoal, cancellationToken);

            if (templatePertanyaan is null)
            {
                return Result.Failure<Guid>(TemplatePertanyaanErrors.NotFound(request.IdTemplateSoal));
            }

            Domain.TemplateJawaban.TemplateJawaban? existingTemplateJawaban = await templateJawabanRepository.GetAsync(request.Uuid, cancellationToken);

            if (existingTemplateJawaban is null)
            {
                Result.Failure(TemplateJawabanErrors.NotFound(request.Uuid));
            }

            Result<Domain.TemplateJawaban.TemplateJawaban> asset = Domain.TemplateJawaban.TemplateJawaban.Update(existingTemplateJawaban!)
                         .ChangeTemplateSoal(int.Parse(templatePertanyaan.Id)) //int.Parse(bankSoal.Value.Id)
                         .ChangeJawabanText(request.JawabanText)
                         .ChangeJawabanImg(request.JawabanImg)
                         .Build();

            if (asset.IsFailure)
            {
                return Result.Failure<Guid>(asset.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
