using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UnpakCbt.Modules.Ujian.Infrastructure.Ujian
{
    internal sealed class UjianConfiguration : IEntityTypeConfiguration<Domain.Ujian.Ujian>
    {
        public void Configure(EntityTypeBuilder<Domain.Ujian.Ujian> builder)
        {
            //builder.HasOne<Category>().WithMany();
        }
    }
}
