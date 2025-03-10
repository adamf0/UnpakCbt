using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban
{
    public static class TemplateJawabanErrors
    {
        public static Error InvalidPage() =>
            Error.Problem("TemplateJawaban.InvalidPage", "Page minimum is 1");
        public static Error InvalidPageSize() =>
            Error.Problem("TemplateJawaban.InvalidPageSize", "Page size minimun is 1");
        public static Error InvalidSearchRegistry(string value) =>
            Error.Problem("TemplateJawaban.InvalidSearchRegistry", $"Search column {value} not registered in system");
        public static Error InvalidSortRegistry(string value) =>
            Error.Problem("TemplateJawaban.InvalidSortRegistry", $"Sort column {value} not registered in system");
        public static Error InvalidArgs(string value) =>
            Error.Problem("TemplateJawaban.InvalidArgs", value);

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
