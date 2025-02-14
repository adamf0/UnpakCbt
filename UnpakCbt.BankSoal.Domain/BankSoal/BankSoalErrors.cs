using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.BankSoal.Domain.BankSoal
{
    public static class BankSoalErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("BankSoal.EmptyData", "Data is not found");

        public static Error NotFound(Guid Id) =>
            Error.Problem("BankSoal.NotFound", $"Question bank with identifier {Id} not found");

        public static Error EmptyTitle() =>
            Error.Problem("BankSoal.EmptyTitle", "Field title in question bank can't be empty");
    }
}
