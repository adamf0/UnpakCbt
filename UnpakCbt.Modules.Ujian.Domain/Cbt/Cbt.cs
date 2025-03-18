using System.ComponentModel.DataAnnotations.Schema;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.Ujian.Domain.Cbt
{
    public sealed partial class Cbt : Entity
    {
        private Cbt()
        {
        }

        public int? Id { get; private set; } = null;

        [Column(TypeName = "VARCHAR(36)")]
        public Guid Uuid { get; private set; }
        [Column("id_ujian")]
        public int IdUjian { get; private set; }
        [Column("id_template_soal")]
        public int IdTemplateSoal { get; private set; }
        [Column("jawaban_benar")]
        public int? JawabanBenar { get; private set; }

        public static CbtBuilder Update(Cbt prev) => new CbtBuilder(prev);

        public static Result<Cbt> Create(
        int IdUjian,
        int IdTemplateSoal,
        int? JawabanBenar = null
        )
        {
            var asset = new Cbt
            {
                Uuid = Guid.NewGuid(),
                IdUjian = IdUjian,
                IdTemplateSoal = IdTemplateSoal,
                JawabanBenar = JawabanBenar,
            };

            asset.Raise(new CbtCreatedDomainEvent(asset.Uuid));

            return asset;
        }
    }
}
