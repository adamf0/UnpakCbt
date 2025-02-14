using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.DeleteUjian
{
    public sealed record DeleteUjianCommand(
        Guid uuid,
        string NoReg
    ) : ICommand;
}
