using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian
{
    public sealed record GetUjianQuery(Guid UjianUuid, string NoReg) : IQuery<UjianResponse>;
}
