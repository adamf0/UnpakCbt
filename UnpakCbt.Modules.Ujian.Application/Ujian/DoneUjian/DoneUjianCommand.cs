using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.DoneUjian
{
    public sealed record DoneUjianCommand(
        Guid uuid,
        string NoReg
    ) : ICommand;
}
