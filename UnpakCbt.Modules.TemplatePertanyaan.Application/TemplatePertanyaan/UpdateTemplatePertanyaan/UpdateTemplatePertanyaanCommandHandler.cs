using Microsoft.Extensions.Logging;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;
using UnpakCbt.Modules.BankSoal.PublicApi;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.PublicApi;
using UnpakCbt.Modules.TemplatePertanyaan.Application.Abstractions.Data;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.UpdateTemplatePertanyaan
{
    internal sealed class UpdateTemplatePertanyaanCommandHandler(
    ITemplatePertanyaanRepository templatePertanyaanRepository,
    IUnitOfWork unitOfWork,
    IBankSoalApi bankSoalApi,
    ITemplateJawabanApi templateJawabanApi,
    ILogger<UpdateTemplatePertanyaanCommand> logger)
    : ICommandHandler<UpdateTemplatePertanyaanCommand>
    {
        public async Task<Result> Handle(UpdateTemplatePertanyaanCommand request, CancellationToken cancellationToken)
        {
            BankSoalResponse? bankSoal = await bankSoalApi.GetAsync(request.IdBankSoal, cancellationToken);

            if (bankSoal is null)
            {
                logger.LogError($"BankSoal dengan referensi Uuid {request.IdBankSoal} tidak ditemukan");
                return Result.Failure<Guid>(BankSoalErrors.NotFound(request.IdBankSoal));
            }

            int? JawabanBenar = null;
            if (request.Jawaban != null)
            {
                TemplateJawabanResponse? templateJawaban = await templateJawabanApi.GetAsync(request.Jawaban ?? Guid.Empty, cancellationToken);

                if (templateJawaban is null)
                {
                    logger.LogError($"TemplateJawaban dengan referensi Uuid {request.Jawaban} tidak ditemukan");
                    return Result.Failure<Guid>(TemplateJawabanErrors.NotFound(request.Jawaban ?? Guid.Empty));
                }

                JawabanBenar = int.Parse(templateJawaban.Id);
            }

            Domain.TemplatePertanyaan.TemplatePertanyaan? existingTemplatePertanyaan = await templatePertanyaanRepository.GetAsync(request.Uuid, cancellationToken);

            if (existingTemplatePertanyaan is null)
            {
                logger.LogError($"TemplatePertanyan dengan referensi uuid {request.Uuid} tidak ditemukan");
                return Result.Failure(TemplatePertanyaanErrors.NotFound(request.Uuid));
            }

            Result<Domain.TemplatePertanyaan.TemplatePertanyaan> templatePertanyaan1 = Domain.TemplatePertanyaan.TemplatePertanyaan.Update(existingTemplatePertanyaan!)
                         .ChangeBankSoal(int.Parse(bankSoal.Id)) //int.Parse(bankSoal.Value.Id)
                         .ChangeTipe(request.Tipe)
                         .ChangePertanyaanText(request.Pertanyaan)
                         .ChangePertanyaanImg(request.Gambar)
                         .ChangeState(request.State)
                         .ChangeJawabanBenar(JawabanBenar) //JawabanBenar
                         .ChangeBobot(request.Bobot)
                         .Build();

            if (templatePertanyaan1.IsFailure)
            {
                logger.LogError("domain bisnis TemplatePertanyan tidak sesuai aturan");
                return Result.Failure<Guid>(templatePertanyaan1.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"berhasil update TemplatePertanyaan dengan referensi Uuid {request.Uuid}");

            return Result.Success();
        }
    }
}
