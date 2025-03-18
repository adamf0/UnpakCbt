using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetAllUjian
{
    public sealed record GetAllUjianByJadwalUjianQuery(Guid uuidJadwalUjian) : IQuery<List<UjianDetailResponse>>;
}
