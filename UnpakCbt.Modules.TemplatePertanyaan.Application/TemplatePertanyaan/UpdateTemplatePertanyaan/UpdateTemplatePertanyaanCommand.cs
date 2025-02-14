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
        int Bobot,
        string? State
    ) : ICommand;
}
