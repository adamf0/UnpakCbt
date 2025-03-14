﻿using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.CreateJadwalUjian
{
    public sealed record CreateJadwalUjianCommand(
        string? Deskripsi,
        int Kuota,
        string Tanggal,
        string JamMulai,
        string JamAkhir,
        Guid IdBankSoal
    ) : ICommand<Guid>;
}
