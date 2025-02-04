using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian
{
    public sealed record GetUjianQuery(Guid UjianUuid) : IQuery<UjianResponse>;
}
