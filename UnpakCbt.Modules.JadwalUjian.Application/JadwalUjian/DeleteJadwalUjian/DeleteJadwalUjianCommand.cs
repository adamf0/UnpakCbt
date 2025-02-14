using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.DeleteJadwalUjian
{
    public sealed record DeleteJadwalUjianCommand(
        Guid uuid
    ) : ICommand;
}
