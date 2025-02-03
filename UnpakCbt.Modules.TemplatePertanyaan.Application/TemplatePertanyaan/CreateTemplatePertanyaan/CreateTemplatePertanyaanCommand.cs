using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.CreateTemplatePertanyaan
{
    public sealed record CreateTemplatePertanyaanCommand(
        Guid IdBankSoal,
        string Tipe,
        string? Pertanyaan,
        string? Gambar,
        Guid? Jawaban,
        int? Bobot,
        string State
    ) : ICommand<Guid>;
}
