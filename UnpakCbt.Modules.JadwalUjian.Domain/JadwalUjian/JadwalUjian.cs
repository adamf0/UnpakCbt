using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian
{
    public sealed partial class JadwalUjian : Entity
    {
        private JadwalUjian()
        {
        }

        public int? Id { get; private set; } = null;

        [Column(TypeName = "VARCHAR(36)")]
        public Guid Uuid { get; private set; }

        public string? Deskripsi { get; private set; } = null!;
        public int Kuota { get; private set; }
        public string Tanggal { get; private set; }
        public string JamMulai { get; private set; }
        public string JamAkhir { get; private set; }
        public int IdBankSoal { get; private set; }

        public static JadwalUjianBuilder Update(JadwalUjian prev) => new JadwalUjianBuilder(prev);

        public static Result<JadwalUjian> Create(
        string? Deskripsi,
        int Kuota,
        string Tanggal,
        string JamMulai,
        string JamAkhir,
        int IdBankSoal
        )
        {
            if (!DateTime.TryParseExact(Tanggal + " " + JamMulai, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var mulai))
            {
                return Result.Failure<JadwalUjian>(JadwalUjianErrors.InvalidScheduleFormat("start"));
            }

            if (!DateTime.TryParseExact(Tanggal + " " + JamAkhir, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var akhir))
            {
                return Result.Failure<JadwalUjian>(JadwalUjianErrors.InvalidScheduleFormat("end"));
            }

            if (IdBankSoal <= 0) {
                return Result.Failure<JadwalUjian>(JadwalUjianErrors.IdBankSoalNotFound(IdBankSoal));
            }

            if (Kuota < -1)
            {
                return Result.Failure<JadwalUjian>(JadwalUjianErrors.KuotaInvalid());
            }

            var asset = new JadwalUjian
            {
                Uuid = Guid.NewGuid(),
                Deskripsi = Deskripsi,
                Kuota = Kuota,
                Tanggal = Tanggal,
                JamMulai = JamMulai,
                JamAkhir = JamAkhir,
                IdBankSoal = IdBankSoal
            };

            asset.Raise(new JadwalUjianCreatedDomainEvent(asset.Uuid));

            return asset;
        }
    }
}
