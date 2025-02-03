using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian
{
    public static class JadwalUjianErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("JadwalUjianErrors.EmptyData", $"data is not found");

        public static Error NotFound(Guid Id) =>
            Error.NotFound("JadwalUjianErrors.NotFound", $"The event with the identifier {Id} was not found");
    }
}
