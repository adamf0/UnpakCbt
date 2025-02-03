using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban
{
    public static class TemplateJawabanErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("TemplateJawaban.EmptyData", $"data is not found");

        public static Error NotFound(Guid Id) =>
            Error.NotFound("TemplateJawaban.NotFound", $"The event with the identifier {Id} was not found");

    }
}
