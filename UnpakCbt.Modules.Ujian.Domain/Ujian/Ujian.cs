using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.Ujian
{
    public sealed partial class Ujian : Entity
    {
        private Ujian()
        {
        }

        public int? Id { get; private set; } = null;

        [Column(TypeName = "VARCHAR(36)")]
        public Guid Uuid { get; private set; }

        public string NoReg { get; private set; } = null!;
        public int IdJadwalUjian { get; private set; }
        public string Status { get; private set; }

        public static UjianBuilder Update(Ujian prev) => new UjianBuilder(prev);

        public static Result<Ujian> Create(
        string NoReg,
        int IdJadwalUjian,
        string Status = "active"
        )
        {
            var asset = new Ujian
            {
                Uuid = Guid.NewGuid(),
                NoReg = NoReg,
                IdJadwalUjian = IdJadwalUjian,
                Status = Status,
            };

            asset.Raise(new UjianCreatedDomainEvent(asset.Uuid));

            return asset;
        }
    }
}
