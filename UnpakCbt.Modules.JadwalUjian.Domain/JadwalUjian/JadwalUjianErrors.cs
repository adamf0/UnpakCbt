using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian
{
    public static class JadwalUjianErrors
    {
        public static Error InvalidPage() =>
            Error.Problem("JadwalUjian.InvalidPage", "Page minimum is 1");
        public static Error InvalidPageSize() =>
            Error.Problem("JadwalUjian.InvalidPageSize", "Page size minimun is 1");
        public static Error InvalidSearchRegistry(string value) =>
            Error.Problem("JadwalUjian.InvalidSearchRegistry", $"Search column {value} not registered in system");
        public static Error InvalidSortRegistry(string value) =>
            Error.Problem("JadwalUjian.InvalidSortRegistry", $"Sort column {value} not registered in system");
        public static Error InvalidArgs(string value) =>
            Error.Problem("JadwalUjian.InvalidArgs", value);

        public static Error EmptyData() =>
            Error.NotFound("JadwalUjianErrors.EmptyData", "Data is not found");

        public static Error NotFound(Guid Id) =>
            Error.Problem("JadwalUjianErrors.NotFound", $"Exam schedule with identifier {Id} not found");

        public static Error NotAvailable(Guid BankSoalUuid) =>
            Error.Problem("JadwalUjianErrors.NotAvailable", $"Exam schedule with identifier BankSoal {BankSoalUuid} not available");

        public static Error NotFoundActive() =>
            Error.Problem("JadwalUjianErrors.NotFoundActive", "No active exam schedule found");

        public static Error IdNotFound(int Id) =>
            Error.Problem("JadwalUjianErrors.IdNotFound", $"Exam schedule with refference id {Id} not found");

        public static Error IdBankSoalNotFound(int IdBankSoal) =>
            Error.Problem("JadwalUjianErrors.IdBankSoalNotFound", $"Exam schedule with refference BankSoal {IdBankSoal} not found");

        public static Error KuotaInvalid() =>
            Error.Problem("JadwalUjianErrors.KuotaInvalid", "Quota value on Exam schedule is invalid");

        public static Error EmptyDataScheduleFormat() =>
           Error.Problem("JadwalUjianErrors.EmptyDataScheduleFormat", "The date, start time and end time fields on the exam schedule cannot be empty.");
           
        public static Error InvalidScheduleFormat(string type) =>
            Error.Problem("JadwalUjianErrors.InvalidScheduleFormat", $"error occurred parsing {type} exam schedule time");
    }
}
