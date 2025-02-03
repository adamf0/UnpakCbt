using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.BankSoal.Domain.BankSoal
{
    public static class BankSoalErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("BankSoal.EmptyData", $"data is not found");

        public static Error NotFound(Guid Id) =>
            Error.NotFound("BankSoal.NotFound", $"The event with the identifier {Id} was not found");
    }
}
