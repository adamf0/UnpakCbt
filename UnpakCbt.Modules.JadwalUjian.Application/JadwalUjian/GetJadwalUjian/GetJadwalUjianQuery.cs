using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian
{
    public sealed record GetJadwalUjianQuery(Guid JadwalUjianUuid) : IQuery<JadwalUjianResponse>;
}
