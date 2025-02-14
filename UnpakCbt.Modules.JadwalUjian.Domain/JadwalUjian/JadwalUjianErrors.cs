using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian
{
    public static class JadwalUjianErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("JadwalUjianErrors.EmptyData", "Data is not found");

        public static Error NotFound(Guid Id) =>
            Error.Problem("JadwalUjianErrors.NotFound", $"Exam schedule with identifier {Id} not found");

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
