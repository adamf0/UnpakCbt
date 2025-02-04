using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.JadwalUjian.PublicApi
{
    public sealed record JadwalUjianResponse(string Id, string Uuid, string? Deskripsi, int Kuota, string Tanggal, string JamMulai, string JamAkhir, int IdBankSoal);
}
