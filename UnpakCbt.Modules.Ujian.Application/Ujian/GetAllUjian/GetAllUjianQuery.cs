using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetAllUjian
{
    public sealed record GetAllUjianQuery() : IQuery<List<UjianResponse>>;
}
