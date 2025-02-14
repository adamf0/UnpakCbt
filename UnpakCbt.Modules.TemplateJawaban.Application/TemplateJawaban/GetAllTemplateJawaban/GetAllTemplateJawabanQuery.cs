using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetAllTemplateJawaban
{
    public sealed record GetAllTemplateJawabanQuery() : IQuery<List<TemplateJawabanResponse>>;
}
