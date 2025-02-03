using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.UpdateTemplatePertanyaan
{
    public sealed record UpdateTemplatePertanyaanCommand(
        Guid Uuid,
        Guid IdBankSoal,
        string Tipe,
        string? Pertanyaan,
        string? Gambar,
        Guid? Jawaban,
        string? State
    ) : ICommand;
}
