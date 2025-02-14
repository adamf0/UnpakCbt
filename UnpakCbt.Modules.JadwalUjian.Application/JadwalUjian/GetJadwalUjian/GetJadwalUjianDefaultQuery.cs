using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian
{
    public sealed record GetJadwalUjianDefaultQuery(Guid JadwalUjianUuid) : IQuery<JadwalUjianDefaultResponse>;
}
