﻿using System.ComponentModel.DataAnnotations.Schema;
using UnpakCbt.Common.Domain;
using UnpakCbt.Modules.Ujian.Domain.TemplatePertanyaan;

namespace UnpakCbt.Modules.Ujian.Domain.TemplatePertanyaan
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
            if (IdBankSoal <= 0)
            {
                return Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.IdBankSoalNotFound(IdBankSoal));
            }
            if (string.IsNullOrWhiteSpace(Tipe))
            {
                return Result.Failure<TemplatePertanyaan>(TemplatePertanyaanErrors.TipeNotFound(Tipe));
            }

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
