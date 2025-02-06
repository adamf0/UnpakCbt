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
            Error.NotFound("TemplateJawaban.EmptyData", "Data is not found");

        public static Error NotFound(Guid Id) =>
            Error.Problem("TemplateJawaban.NotFound", $"Answer with identifier {Id} not found");

        public static Error IdTemplateSoalNotFound(int IdTemplateSoal) =>
            Error.Problem("TemplateJawaban.IdTemplateSoalNotFound", $"Question template reference {IdTemplateSoal} not found in answers");

        public static Error ImgTextNotEmpty() =>
            Error.Problem("TemplateJawaban.ImgTextNotEmpty", "Image or text answer references in the answer cannot be empty");

    }
}
