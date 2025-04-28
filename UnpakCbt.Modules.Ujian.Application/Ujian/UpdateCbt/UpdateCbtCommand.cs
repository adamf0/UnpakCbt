using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.UpdateCbt
{
    public sealed record UpdateCbtCommand(
        Guid UuidUjian,
        string NoReg,
        Guid uuidTemplateSoal,
        Guid uuidJawabanBenar,
        string? Mode
    ) : ICommand;
}
