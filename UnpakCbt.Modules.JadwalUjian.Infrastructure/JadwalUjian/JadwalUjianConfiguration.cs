using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace UnpakCbt.Modules.JadwalUjian.Infrastructure.JadwalUjian
{
    internal sealed class JadwalUjianConfiguration : IEntityTypeConfiguration<Domain.JadwalUjian.JadwalUjian>
    {
        public void Configure(EntityTypeBuilder<Domain.JadwalUjian.JadwalUjian> builder)
        {
            //builder.HasOne<Category>().WithMany();
        }
    }
}
