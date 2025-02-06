using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.Ujian
{
    public static class CbtErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("CbtErrors.EmptyData", $"data is not found");

        public static Error NotFound(Guid Id) =>
            Error.NotFound("CbtErrors.NotFound", $"The event with the identifier {Id} was not found");
    }
}
