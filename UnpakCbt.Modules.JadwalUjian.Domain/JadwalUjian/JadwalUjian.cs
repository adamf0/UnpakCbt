using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
