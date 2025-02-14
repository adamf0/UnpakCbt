using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.UpdateBankSoal
{
    public sealed record UpdateBankSoalCommand(
        Guid Uuid,
        string Judul,
        string? Rule = null
    ) : ICommand;
}
