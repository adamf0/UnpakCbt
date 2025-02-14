using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.GetAllBankSoal
{
    public sealed record GetAllBankSoalQuery() : IQuery<List<BankSoalResponse>>;
}
