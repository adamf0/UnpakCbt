using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;
using UnpakCbt.Modules.BankSoal.PublicApi;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.PublicApi;
using UnpakCbt.Modules.TemplatePertanyaan.Application.Abstractions.Data;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.CreateTemplatePertanyaan
{
    internal sealed class CreateTemplatePertanyaanCommandHandler(
    ITemplatePertanyaanRepository templatePertanyaanRepository,
    IUnitOfWork unitOfWork,
    IBankSoalApi bankSoalApi,
    ITemplateJawabanApi templateJawabanApi)
    : ICommandHandler<CreateTemplatePertanyaanCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateTemplatePertanyaanCommand request, CancellationToken cancellationToken)
        {
            BankSoalResponse? bankSoal = await bankSoalApi.GetAsync(request.IdBankSoal, cancellationToken);

            if (bankSoal is null)
            {
                return Result.Failure<Guid>(BankSoalErrors.NotFound(request.IdBankSoal));
            }

            int? JawabanBenar = null;
            if (request.Jawaban != null)
            {
                TemplateJawabanResponse? templateJawaban = await templateJawabanApi.GetAsync(request.Jawaban ?? Guid.Empty, cancellationToken);

                if (templateJawaban is null)
                {
                    return Result.Failure<Guid>(TemplateJawabanErrors.NotFound(request.Jawaban ?? Guid.Empty));
                }

                JawabanBenar = int.Parse(templateJawaban.Id);
            }

            Result<Domain.TemplatePertanyaan.TemplatePertanyaan> result = Domain.TemplatePertanyaan.TemplatePertanyaan.Create(
                int.Parse(bankSoal.Id), //int.Parse(bankSoal.Value.Id)
                request.Tipe,
                request.Pertanyaan,
                request.Gambar,
                JawabanBenar,
                request.State
            );

            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }

            templatePertanyaanRepository.Insert(result.Value);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return result.Value.Uuid;
        }
    }
}
