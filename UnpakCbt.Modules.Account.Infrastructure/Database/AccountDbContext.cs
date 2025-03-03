using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnpakCbt.Modules.Account.Application.Abstractions.Data;
using UnpakCbt.Modules.Account.Infrastructure.Account;

namespace UnpakCbt.Modules.Account.Infrastructure.Database
{
    public sealed class AccountDbContext(DbContextOptions<AccountDbContext> options) : DbContext(options), IUnitOfWork
    {
        internal DbSet<Domain.Account.Account> Account { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Account.Account>().ToTable(Schemas.Account);
            modelBuilder.ApplyConfiguration(new AccountConfiguration());

            modelBuilder.Entity<Domain.Account.Account>(entity =>
            {
                var guidConverter = new ValueConverter<Guid, string>(
                    v => v.ToString("D"), // Mengonversi Guid ke string dengan format "N" (tidak ada tanda hubung)
                    v => Guid.ParseExact(v, "D") // Mengonversi string kembali menjadi Guid
                );
                entity.ToTable(Schemas.Account);

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Uuid)
                      .HasColumnName("uuid")
                      .HasColumnType("VARCHAR(36)");
                //.HasConversion(guidConverter);

                entity.Property(e => e.Username)
                      .HasColumnName("username");

                entity.Property(e => e.Password)
                      .HasColumnName("password");

                entity.Property(e => e.Level)
                      .HasColumnName("level");

                entity.Property(e => e.Status)
                      .HasColumnName("status");
            });
        }
    }
}
