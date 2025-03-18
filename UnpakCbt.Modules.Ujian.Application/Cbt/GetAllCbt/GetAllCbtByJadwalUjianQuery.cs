using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Modules.Ujian.Application.Cbt.GetCbt;

namespace UnpakCbt.Modules.Ujian.Application.Cbt.GetAllCbt
{
    public sealed record GetAllCbtByJadwalUjianQuery(Guid uuidUjian) : IQuery<List<CbtResponse>>;
}
