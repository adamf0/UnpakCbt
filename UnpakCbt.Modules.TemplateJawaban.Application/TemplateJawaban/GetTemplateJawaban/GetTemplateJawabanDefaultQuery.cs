using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban
{
    public sealed record GetTemplateJawabanDefaultQuery(Guid TemplateJawabanUuid) : IQuery<TemplateJawabanDefaultResponse>;
}
