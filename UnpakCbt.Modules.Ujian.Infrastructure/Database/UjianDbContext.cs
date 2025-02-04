using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnpakCbt.Modules.Ujian.Application.Abstractions.Data;
using UnpakCbt.Modules.Ujian.Infrastructure.Ujian;

namespace UnpakCbt.Modules.Ujian.Infrastructure.Database
{
    public sealed class UjianDbContext(DbContextOptions<UjianDbContext> options) : DbContext(options), IUnitOfWork
    {
        internal DbSet<Domain.Ujian.Ujian> Ujian { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }
    }
}
