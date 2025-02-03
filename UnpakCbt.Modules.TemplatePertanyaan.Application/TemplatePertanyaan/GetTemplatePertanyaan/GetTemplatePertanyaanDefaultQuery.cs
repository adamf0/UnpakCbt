using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan
{
    public sealed record GetTemplatePertanyaanDefaultQuery(Guid TemplatePertanyaanUuid) : IQuery<TemplatePertanyaanDefaultResponse>;
}
