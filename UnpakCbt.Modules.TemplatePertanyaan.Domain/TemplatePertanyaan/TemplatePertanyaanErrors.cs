using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan
{
    public static class TemplatePertanyaanErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("TemplatePertanyaan.EmptyData", $"data is not found");

        public static Error NotFound(Guid Id) =>
            Error.NotFound("TemplatePertanyaan.NotFound", $"The event with the identifier {Id} was not found");

    }
}
