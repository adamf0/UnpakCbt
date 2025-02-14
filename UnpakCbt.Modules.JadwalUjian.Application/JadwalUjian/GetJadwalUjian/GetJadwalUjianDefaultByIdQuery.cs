using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.GetJadwalUjian
{
    public sealed record GetJadwalUjianDefaultByIdQuery(int? id) : IQuery<JadwalUjianDefaultResponse>;
}
