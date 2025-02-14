using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal
{
    public sealed record GetBankSoalDefaultQuery(Guid BankSoalUuid) : IQuery<BankSoalDefaultResponse>;
}
