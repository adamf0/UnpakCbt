using System.ComponentModel.DataAnnotations.Schema;
using UnpakCbt.Common.Domain;

namespace UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan
{
    public sealed partial class TemplatePertanyaan : Entity
    {
        private TemplatePertanyaan()
        {
        }

        public int? Id { get; private set; } = null;

        [Column(TypeName = "VARCHAR(36)")]
        public Guid Uuid { get; private set; }

        public int IdBankSoal { get; private set; }
        public string Tipe { get; private set; }
        [Column("pertanyaan_text")]
        public string? PertanyaanText { get; private set; }
        [Column("pertanyaan_img")]
        public string? PertanyaanImg { get; private set; } //file path
        [Column("jawaban_benar")]
        public int? JawabanBenar { get; private set; } //id jawaban
        public int? Bobot { get; private set; } //null / 0...n
        public string? State { get; private set; } = null; //init/null

        public static TemplatePertanyaanBuilder Update(TemplatePertanyaan prev) => new TemplatePertanyaanBuilder(prev);

        public static Result<TemplatePertanyaan> Create(
        int IdBankSoal,
        string Tipe,
        string? PertanyaanText = null,
        string? PertanyaanImg = null,
        int? JawabanBenar = null,
        int? Bobot = null,
        string? State = null
        )
        {
            var asset = new TemplatePertanyaan
            {
                Uuid = Guid.NewGuid(),
                IdBankSoal = IdBankSoal,
                Tipe = Tipe,
                PertanyaanText = PertanyaanText,
                PertanyaanImg = PertanyaanImg,
                JawabanBenar = JawabanBenar,
                Bobot = Bobot,
                State = State
            };

            asset.Raise(new TemplatePertanyaanCreatedDomainEvent(asset.Uuid));

            return asset;
        }
    }
}
