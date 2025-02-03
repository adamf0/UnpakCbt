using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.GetBankSoal
{
    public sealed record GetBankSoalDefaultQuery(Guid BankSoalUuid) : IQuery<BankSoalDefaultResponse>;
}
