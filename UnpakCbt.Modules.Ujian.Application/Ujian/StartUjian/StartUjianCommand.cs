using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.StartUjian
{
    public sealed record StartUjianCommand(
        Guid uuid,
        string NoReg
    ) : ICommand;
}
