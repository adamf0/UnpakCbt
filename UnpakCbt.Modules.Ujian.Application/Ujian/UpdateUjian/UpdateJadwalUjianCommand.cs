using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.UpdateUjian
{
    public sealed record UpdateUjianCommand(
        Guid Uuid,
        string NoReg,
        Guid IdJadwalUjian,
        string Status
    ) : ICommand;
}
