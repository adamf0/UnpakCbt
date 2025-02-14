using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal
{
    public sealed record GetBankSoalQuery(Guid BankSoalUuid) : IQuery<BankSoalResponse>;
}
