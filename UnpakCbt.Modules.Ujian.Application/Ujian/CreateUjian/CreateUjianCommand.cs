using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.CreateUjian
{
    public sealed record CreateUjianCommand(
        string NoReg,
        Guid IdJadwalUjian
    ) : ICommand<Guid>;
}
