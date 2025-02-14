using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UnpakCbt.Modules.TemplateJawaban.Infrastructure.TemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Application.Abstractions.Data;

namespace UnpakCbt.Modules.TemplateJawaban.Infrastructure.Database
{
    public sealed class TemplateJawabanDbContext(DbContextOptions<TemplateJawabanDbContext> options) : DbContext(options), IUnitOfWork
    {
        internal DbSet<Domain.TemplateJawaban.TemplateJawaban> TemplateJawaban { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.TemplateJawaban.TemplateJawaban>().ToTable(Schemas.TemplateJawaban);
            modelBuilder.ApplyConfiguration(new TemplateJawabanConfiguration());

            modelBuilder.Entity<Domain.TemplateJawaban.TemplateJawaban>(entity =>
            {
                var guidConverter = new ValueConverter<Guid, string>(
                    v => v.ToString("D"), // Mengonversi Guid ke string dengan format "N" (tidak ada tanda hubung)
                    v => Guid.ParseExact(v, "D") // Mengonversi string kembali menjadi Guid
                );
                entity.ToTable(Schemas.TemplateJawaban);

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Uuid)
                      .HasColumnName("uuid")
                      .HasColumnType("VARCHAR(36)");
                //.HasConversion(guidConverter);

                entity.Property(e => e.JawabanText)
                      .HasColumnName("jawaban_text");

                entity.Property(e => e.JawabanImg)
                      .HasColumnName("jawaban_img");

                entity.Property(e => e.IdTemplateSoal)
                      .HasColumnName("id_template_soal");
            });
        }
    }
}
