using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.UpdateTemplatePertanyaan
{
    public sealed record RemoveImageTemplatePertanyaanCommand(
        Guid Uuid
    ) : ICommand;
}
