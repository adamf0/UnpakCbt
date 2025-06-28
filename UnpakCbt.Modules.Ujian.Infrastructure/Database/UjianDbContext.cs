using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.Ujian.Infrastructure.Ujian;

namespace UnpakCbt.Modules.Ujian.Infrastructure.Database
{
    public sealed class UjianDbContext(DbContextOptions<UjianDbContext> options) : DbContext(options), IUnitOfWork
    {
        internal DbSet<Domain.LogEvent.Log> Log { get; set; }
        internal DbSet<Domain.Ujian.Ujian> Ujian { get; set; }
        internal DbSet<Domain.Cbt.Cbt> Cbt { get; set; }
        internal DbSet<Domain.TemplatePertanyaan.TemplatePertanyaan> TemplatePertanyaan { get; set; }
        internal DbSet<Domain.JadwalUjian.JadwalUjian> JadwalUjian { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.JadwalUjian.JadwalUjian>().ToTable(Schemas.JadwalUjian);
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


            ////


            modelBuilder.Entity<Domain.TemplatePertanyaan.TemplatePertanyaan>().ToTable(Schemas.TemplatePertanyaan);
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

                entity.Property(e => e.Bobot)
                      .HasColumnName("bobot");

                entity.Property(e => e.State)
                      .HasColumnName("state");
            });


            ////

            modelBuilder.Entity<Domain.Ujian.Ujian>().ToTable(Schemas.Ujian);
            modelBuilder.ApplyConfiguration(new UjianConfiguration());
            modelBuilder.Entity<Domain.Ujian.Ujian>(entity =>
            {
                var guidConverter = new ValueConverter<Guid, string>(
                    v => v.ToString("D"), // Mengonversi Guid ke string dengan format "N" (tidak ada tanda hubung)
                    v => Guid.ParseExact(v, "D") // Mengonversi string kembali menjadi Guid
                );
                entity.ToTable(Schemas.Ujian);

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Uuid)
                      .HasColumnName("uuid")
                      .HasColumnType("VARCHAR(36)");
                //.HasConversion(guidConverter);

                entity.Property(e => e.NoReg)
                      .HasColumnName("no_reg");

                entity.Property(e => e.IdJadwalUjian)
                      .HasColumnName("id_jadwal_ujian");

                entity.Property(e => e.Status)
                      .HasColumnName("status");
            });


            ////
            
            modelBuilder.Entity<Domain.Cbt.Cbt>().ToTable(Schemas.Cbt);
            modelBuilder.ApplyConfiguration(new UjianConfiguration());
            modelBuilder.Entity<Domain.Cbt.Cbt>(entity =>
            {
                var guidConverter = new ValueConverter<Guid, string>(
                    v => v.ToString("D"), // Mengonversi Guid ke string dengan format "N" (tidak ada tanda hubung)
                    v => Guid.ParseExact(v, "D") // Mengonversi string kembali menjadi Guid
                );
                entity.ToTable(Schemas.Cbt);

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Uuid)
                      .HasColumnName("uuid")
                      .HasColumnType("VARCHAR(36)");
                //.HasConversion(guidConverter);

                entity.Property(e => e.IdUjian)
                      .HasColumnName("id_ujian");

                entity.Property(e => e.IdTemplateSoal)
                      .HasColumnName("id_template_soal");

                entity.Property(e => e.JawabanBenar)
                      .HasColumnName("jawaban_benar");

                entity.Property(e => e.Trial)
                      .HasColumnName("trial");
            });

            ////

            modelBuilder.Entity<Domain.LogEvent.Log>().ToTable(Schemas.Log);
            modelBuilder.ApplyConfiguration(new UjianConfiguration());
            modelBuilder.Entity<Domain.LogEvent.Log>(entity =>
            {
                var guidConverter = new ValueConverter<Guid, string>(
                    v => v.ToString("D"), // Mengonversi Guid ke string dengan format "N" (tidak ada tanda hubung)
                    v => Guid.ParseExact(v, "D") // Mengonversi string kembali menjadi Guid
                );
                entity.ToTable(Schemas.Log);

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Uuid)
                      .HasColumnName("uuid")
                      .HasColumnType("VARCHAR(36)");
                //.HasConversion(guidConverter);

                entity.Property(e => e.NoReg)
                      .HasColumnName("noreg");

                entity.Property(e => e.Events)
                      .HasColumnName("event");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at");
            });
        }
    }
}
