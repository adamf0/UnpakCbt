using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.Ujian
{
    public static class CbtErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("CbtErrors.EmptyData", "Data is not found");

        public static Error NotFound(Guid Id) =>
            Error.NotFound("CbtErrors.NotFound", $"Cbt with identifier {Id} not found");
    }
}
