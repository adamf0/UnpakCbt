using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.DeleteBankSoal
{
    public sealed record DeleteBankSoalCommand(
        Guid uuid
    ) : ICommand;
}
