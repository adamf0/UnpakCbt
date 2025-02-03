using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Modules.TemplatePertanyaan.Infrastructure.TemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Application.Abstractions.Data;

namespace UnpakCbt.Modules.TemplatePertanyaan.Infrastructure.Database
{
    public sealed class TemplatePertanyaanDbContext(DbContextOptions<TemplatePertanyaanDbContext> options) : DbContext(options), IUnitOfWork
    {
        internal DbSet<Domain.TemplatePertanyaan.TemplatePertanyaan> TemplatePertanyaan { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.TemplatePertanyaan.TemplatePertanyaan>().ToTable(Schemas.TemplatePertanyaan);
            modelBuilder.ApplyConfiguration(new TemplatePertanyaanConfiguration());

            modelBuilder.Entity<Domain.TemplatePertanyaan.TemplatePertanyaan>(entity =>
            {
                var guidConverter = new ValueConverter<Guid, string>(
                    v => v.ToString("D"), // Mengonversi Guid ke string dengan format "N" (tidak ada tanda hubung)
                    v => Guid.ParseExact(v, "D") // Mengonversi string kembali menjadi Guid
                );
                entity.ToTable(Schemas.TemplatePertanyaan);

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Uuid)
                      .HasColumnName("uuid")
                      .HasColumnType("VARCHAR(36)");
                //.HasConversion(guidConverter);

                entity.Property(e => e.IdBankSoal)
                      .HasColumnName("id_bank_soal");

                entity.Property(e => e.Tipe)
                      .HasColumnName("tipe");

                entity.Property(e => e.PertanyaanText)
                      .HasColumnName("pertanyaan_text");

                entity.Property(e => e.PertanyaanImg)
                      .HasColumnName("pertanyaan_img");

                entity.Property(e => e.JawabanBenar)
                      .HasColumnName("jawaban_benar");

                entity.Property(e => e.State)
                      .HasColumnName("state");
            });
        }
    }
}
