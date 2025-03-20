using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.CancelUjian
{
    public sealed record CancelUjianCommand(
        Guid uuid,
        string noReg
    ) : ICommand;
}
