using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Laporan.Domain.Laporan
{
    public static class LaporanErrors
    {
        public static Error EmptyData() =>
            Error.NotFound("Laporan.EmptyData", "Data is not found");

        /*public static Error NotFound(Guid Id) =>
            Error.Problem("Laporan.NotFound", $"Laporan with identifier jadwal_ujian {Id} not found");*/
    }
}
