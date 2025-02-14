using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan
{
    public sealed record GetTemplatePertanyaanQuery(Guid TemplatePertanyaanUuid) : IQuery<TemplatePertanyaanResponse>;
}
