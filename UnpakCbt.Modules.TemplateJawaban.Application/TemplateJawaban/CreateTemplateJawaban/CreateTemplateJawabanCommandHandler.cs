using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    ITemplatePertanyaanApi templatePertanyaanApi)
    : ICommandHandler<CreateTemplateJawabanCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateTemplateJawabanCommand request, CancellationToken cancellationToken)
        {
            TemplatePertanyaanResponse? templatePertanyaan = await templatePertanyaanApi.GetAsync(request.IdTemplateSoal, cancellationToken);

            if (templatePertanyaan is null)
            {
                return Result.Failure<Guid>(TemplatePertanyaanErrors.NotFound(request.IdTemplateSoal));
            }

            Result<Domain.TemplateJawaban.TemplateJawaban> result = Domain.TemplateJawaban.TemplateJawaban.Create(
                int.Parse(templatePertanyaan.Id), //int.Parse(bankSoal.Value.Id)
                request.JawabanText,
                request.JawabanImg
            );

            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }

            templateJawabanRepository.Insert(result.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return result.Value.Uuid;
        }
    }
}
