using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.Ujian
{
    public static class UjianErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("UjianErrors.EmptyData", "Data is not found");

        public static Error NotFound(Guid Id) =>
            Error.Problem("UjianErrors.NotFound", $"Exam schedule with identifier {Id} not found");

        public static Error IdJawabanBenarNotFound(Guid IdJawabanBenar) =>
            Error.Problem("UjianErrors.IdJawabanBenarNotFound", $"Exam schedule with reference IdjawabanBenar {IdJawabanBenar} not found");

        public static Error IdJadwalUjianNotFound(int Id) =>
            Error.Problem("UjianErrors.IdJadwalUjianNotFound", $"Exam schedule with reference IdJadwalUjian {Id} not found");

        public static Error NoRegNotEmpty() =>
            Error.Problem("TemplatePertanyaan.NoRegNotEmpty", "The registration reference number in the exam schedule cannot be empty");

        public static Error ScheduleExamNotFound(Guid Id) =>
            Error.Problem("UjianErrors.ScheduleExamNotFound", $"Schedule exam with the identifier {Id} was not found");

        public static Error ScheduleExamNoStartExam() =>
            Error.Problem("UjianErrors.ScheduleExamNoStartExam", "User has not started entering the exam");

        public static Error FailDecrement(string key) =>
            Error.Problem("UjianErrors.FailDecrement", $"The reference key {key} fail do decrement");

        public static Error IncorrectReferenceNoReg(Guid Id, string NoReg) =>
            Error.Problem("UjianErrors.IncorrectReferenceNoReg", $"The {Id} reference to the registration number {NoReg} on the exam schedule does not match the actual data");

        public static Error ScheduleExamCancelExam() =>
            Error.Problem("UjianErrors.ScheduleExamCancelExam", "The status exam schedule has been cancelled");

        public static Error ScheduleExamDoneExam() =>
            Error.Problem("UjianErrors.ScheduleExamDoneExam", "The status exam schedule has been done");

        public static Error ScheduleExamNotStartedExam() =>
            Error.Problem("UjianErrors.ScheduleExamNotStartedExam", "The status exam schedule has not started yet");

        public static Error ScheduleExamStartExam() =>
            Error.Problem("UjianErrors.ScheduleExamStartExam", "The status exam schedule has been started");

        public static Error OutRange(string Mulai, string Akhir) =>
            Error.Problem("UjianErrors.OutRange", $"Request cancelled because exam on {Mulai} to {Akhir} is still ongoing");

        public static Error OutRangeExam(string Mulai, string Akhir) =>
            Error.Problem("UjianErrors.OutRangeExam", $"Request cancelled because exam on {Mulai} to {Akhir}");

        public static Error EmptyDataScheduleFormat() =>
            Error.Problem("UjianErrors.EmptyDataScheduleFormat", "The registration reference tanggal, jam mulai & jam keluar in the exam schedule cannot be empty");

        public static Error InvalidScheduleFormat(string type) =>
            Error.Problem("UjianErrors.InvalidScheduleFormat", $"error occurred parsing {type} exam schedule time");

        public static Error InvalidRangeDateTime() =>
            Error.Problem("UjianErrors.InvalidRangeDateTime", "start date can't greater than end date");

        public static Error QuotaExhausted(string current, string max) =>
            Error.Problem("UjianErrors.QuotaExhausted", $"Exam schedule bookings have exceeded the available quota ({current}/{max})");

        public static Error QuotaExhausted2(string current, string max) =>
            Error.Problem("UjianErrors.QuotaExhausted[2]", $"Exam schedule bookings have exceeded the available quota ({current}/{max})");

        public static Error NotFoundReference() =>
            Error.Problem("UjianErrors.NotFoundReference", "identifier ujian was not found");

        public static Error ActiveExam(string NoReg) =>
            Error.Problem("UjianErrors.ActiveExam", $"Exam schedule with registration number {NoReg} is still active");
    }
}
