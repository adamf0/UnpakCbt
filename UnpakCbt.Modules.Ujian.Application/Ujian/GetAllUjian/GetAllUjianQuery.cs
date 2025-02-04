using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian;

namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetAllUjian
{
    public sealed record GetAllUjianQuery() : IQuery<List<UjianResponse>>;
}
