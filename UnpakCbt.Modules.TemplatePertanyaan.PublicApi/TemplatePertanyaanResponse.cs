using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.TemplatePertanyaan.PublicApi
{
    public sealed record TemplatePertanyaanResponse(string Id, string Uuid, int IdBankSoal, string Tipe, string? PertanyaanText, string? PertanyaanImg, int? JawabanBenar, string? State);
}
