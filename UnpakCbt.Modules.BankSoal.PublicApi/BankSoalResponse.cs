using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.BankSoal.PublicApi
{
    public sealed record BankSoalResponse(string Id, string Uuid, string Judul, string? Rule);
}
