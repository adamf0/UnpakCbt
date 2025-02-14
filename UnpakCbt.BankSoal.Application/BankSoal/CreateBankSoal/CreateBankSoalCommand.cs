using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.CreateBankSoal
{
    public sealed record CreateBankSoalCommand(
        string Judul,
        string? Rule = null
    ) : ICommand<Guid>;
}
