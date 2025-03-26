using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetAllTemplatePertanyaan
{
    public sealed record GetAllTemplatePertanyaanByBankSoalQuery(Guid BakSoalUuid, string type) : IQuery<List<TemplatePertanyaanResponse>>;
}
