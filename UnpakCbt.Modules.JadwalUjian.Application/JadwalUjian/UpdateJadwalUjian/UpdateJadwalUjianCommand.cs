﻿using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.JadwalUjian.Application.JadwalUjian.UpdateJadwalUjian
{
    public sealed record UpdateJadwalUjianCommand(
        Guid Uuid,
        string? Deskripsi,
        int Kuota,
        string Tanggal,
        string JamMulai,
        string JamAkhir
    ) : ICommand;
}
