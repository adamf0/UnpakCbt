using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban
{
    public sealed record GetTemplateJawabanQuery(Guid TemplateJawabanUuid) : IQuery<TemplateJawabanResponse>;
}
