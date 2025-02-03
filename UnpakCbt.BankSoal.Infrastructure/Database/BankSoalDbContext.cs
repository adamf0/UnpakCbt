using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnpakCbt.Modules.BankSoal.Application.Abstractions.Data;
using UnpakCbt.Modules.BankSoal.Infrastructure.BankSoal;

namespace UnpakCbt.Modules.BankSoal.Infrastructure.Database
{
    public sealed class BankSoalDbContext(DbContextOptions<BankSoalDbContext> options) : DbContext(options), IUnitOfWork
    {
        internal DbSet<Domain.BankSoal.BankSoal> BankSoal { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.BankSoal.BankSoal>().ToTable(Schemas.BankSoal);
            modelBuilder.ApplyConfiguration(new BankSoalConfiguration());

            modelBuilder.Entity<Domain.BankSoal.BankSoal>(entity =>
            {
                var guidConverter = new ValueConverter<Guid, string>(
                    v => v.ToString("D"), // Mengonversi Guid ke string dengan format "N" (tidak ada tanda hubung)
                    v => Guid.ParseExact(v, "D") // Mengonversi string kembali menjadi Guid
                );
                entity.ToTable(Schemas.BankSoal);

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Uuid)
                      .HasColumnName("uuid")
                      .HasColumnType("VARCHAR(36)");
                //.HasConversion(guidConverter);

                entity.Property(e => e.Judul)
                      .HasColumnName("judul");

                entity.Property(e => e.Rule)
                      .HasColumnName("rule");
            });
        }
    }
}
