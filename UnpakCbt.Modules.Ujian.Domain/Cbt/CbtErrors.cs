using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.Cbt
{
    public static class CbtErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("CbtErrors.EmptyData", "Data is not found");

        public static Error NotFound(Guid UuidUjian, Guid uuidTemplateSoal, string NoReg) =>
            Error.NotFound("CbtErrors.NotFound", $"Cbt with identifier UuidUjian {UuidUjian}, uuidTemplateSoal {uuidTemplateSoal} and NoReg {NoReg} not found");
    }
}
