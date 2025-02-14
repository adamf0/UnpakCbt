using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.DeleteTemplatePertanyaan
{
    public sealed record DeleteTemplatePertanyaanCommand(
        Guid uuid
    ) : ICommand;
}
