using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.UpdateJadwalUjian
{
    public sealed record UpdateJadwalUjianCommand(
        Guid Uuid,
        string? Deskripsi,
        int Kuota,
        string Tanggal,
        string JamMulai,
        string JamAkhir,
        Guid IdBankSoal
    ) : ICommand;
}
