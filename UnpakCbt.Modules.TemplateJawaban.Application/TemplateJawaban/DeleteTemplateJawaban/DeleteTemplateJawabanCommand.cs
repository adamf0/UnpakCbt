using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.DeleteTemplateJawaban
{
    public sealed record DeleteTemplateJawabanCommand(
        Guid uuid
    ) : ICommand;
}
