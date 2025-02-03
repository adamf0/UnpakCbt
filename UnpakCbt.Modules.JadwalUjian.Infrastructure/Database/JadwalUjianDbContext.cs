using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnpakCbt.Modules.JadwalUjian.Application.Abstractions.Data;
using UnpakCbt.Modules.JadwalUjian.Infrastructure.JadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Infrastructure.Database
{
    public sealed class JadwalUjianDbContext(DbContextOptions<JadwalUjianDbContext> options) : DbContext(options), IUnitOfWork
    {
        internal DbSet<Domain.JadwalUjian.JadwalUjian> JadwalUjian { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.JadwalUjian.JadwalUjian>().ToTable(Schemas.JadwalUjian);
            modelBuilder.ApplyConfiguration(new JadwalUjianConfiguration());

            modelBuilder.Entity<Domain.JadwalUjian.JadwalUjian>(entity =>
            {
                var guidConverter = new ValueConverter<Guid, string>(
                    v => v.ToString("D"), // Mengonversi Guid ke string dengan format "N" (tidak ada tanda hubung)
                    v => Guid.ParseExact(v, "D") // Mengonversi string kembali menjadi Guid
                );
                entity.ToTable(Schemas.JadwalUjian);

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Uuid)
                      .HasColumnName("uuid")
                      .HasColumnType("VARCHAR(36)");
                //.HasConversion(guidConverter);

                entity.Property(e => e.Deskripsi)
                      .HasColumnName("deskripsi");

                entity.Property(e => e.Kuota)
                      .HasColumnName("kuota");

                entity.Property(e => e.Tanggal)
                      .HasColumnName("tanggal");

                entity.Property(e => e.Kuota)
                      .HasColumnName("kuota");

                entity.Property(e => e.JamMulai)
                      .HasColumnName("jam_mulai_ujian");

                entity.Property(e => e.JamAkhir)
                      .HasColumnName("jam_akhir_ujian");

                entity.Property(e => e.IdBankSoal)
                      .HasColumnName("id_bank_soal");
            });
        }
    }
}
