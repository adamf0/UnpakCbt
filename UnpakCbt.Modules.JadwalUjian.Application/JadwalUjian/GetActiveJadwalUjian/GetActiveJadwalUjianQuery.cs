using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetActiveJadwalUjian
{
    public sealed record GetActiveJadwalUjianQuery() : IQuery<JadwalUjianActiveResponse>;
}
