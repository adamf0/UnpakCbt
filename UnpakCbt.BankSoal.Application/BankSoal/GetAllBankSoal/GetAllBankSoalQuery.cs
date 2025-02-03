using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;
using UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.GetAllBankSoal
{
    public sealed record GetAllBankSoalQuery() : IQuery<List<BankSoalResponse>>;
}
