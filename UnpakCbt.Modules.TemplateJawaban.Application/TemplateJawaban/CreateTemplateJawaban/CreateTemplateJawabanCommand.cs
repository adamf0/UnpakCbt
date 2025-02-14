using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.CreateTemplateJawaban
{
    public sealed record CreateTemplateJawabanCommand(
        Guid IdTemplateSoal,
        string? JawabanText = null,
        string? JawabanImg = null
    ) : ICommand<Guid>;
}
