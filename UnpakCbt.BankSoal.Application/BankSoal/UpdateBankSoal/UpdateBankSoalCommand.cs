using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Application.Messaging;

namespace UnpakCbt.Modules.BankSoal.Application.BankSoal.UpdateBankSoal
{
    public sealed record UpdateBankSoalCommand(
        Guid Uuid,
        string Judul,
        string? Rule = null
    ) : ICommand;
}
