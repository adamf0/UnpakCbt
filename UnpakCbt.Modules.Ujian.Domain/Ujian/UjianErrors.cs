using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.Ujian
{
    public static class UjianErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("UjianErrors.EmptyData", $"data is not found");

        public static Error NotFound(Guid Id) =>
            Error.NotFound("UjianErrors.NotFound", $"The event with the identifier {Id} was not found");

        public static Error ScheduleExamNotFound(Guid Id) =>
            Error.NotFound("UjianErrors.ScheduleExamNotFound", $"Schedule exam with the identifier {Id} was not found");

        public static Error OutRange(string Mulai, string Akhir) =>
            Error.Problem("UjianErrors.OutRange", $"Request cancelled because exam on {Mulai} to {Akhir} is still ongoing");

        public static Error EmptyDataScheduleFormat() =>
            Error.Problem("UjianErrors.EmptyDataScheduleFormat", "Data tanggal, jam mulai & jam keluar is empty");

        public static Error InvalidScheduleFormat(string type) =>
            Error.Problem("UjianErrors.InvalidScheduleFormat", $"There is a format error in ${type}");

        public static Error InvalidRangeDateTime() =>
            Error.Problem("UjianErrors.InvalidRangeDateTime", "start date can't greater than end date");

        public static Error QuotaExhausted(string current, string max) =>
            Error.Problem("UjianErrors.InvalidRangeDateTime", $"Exam schedule bookings have exceeded the available quota ({current}/{max})");
    }
}
