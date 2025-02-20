using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.StatusBankSoal
{
    public sealed record StatusBankSoalCommand(
        Guid Uuid,
        string Status
    ) : ICommand;
}
