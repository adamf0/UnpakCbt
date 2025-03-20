using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.RescheduleUjian
{
    public sealed record RescheduleUjianCommand(
        string NoReg,
        Guid prevIdJadwalUjian,
        Guid newIdJadwalUjian
    ) : ICommand;
}
