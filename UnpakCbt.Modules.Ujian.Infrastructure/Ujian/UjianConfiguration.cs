using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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
