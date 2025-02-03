using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.TemplateJawaban.PublicApi
{
    public sealed record TemplateJawabanResponse(string Id, string Uuid, string? JawabanText, string? JawabanImg);
}
