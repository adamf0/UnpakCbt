using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian
{
    public sealed record GetUjianDefaultQuery(Guid UjianUuid) : IQuery<UjianDefaultResponse>;
}
