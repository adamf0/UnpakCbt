using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetAllJadwalUjian
{
    public sealed record GetAllJadwalUjianQuery() : IQuery<List<JadwalUjianResponse>>;
}
