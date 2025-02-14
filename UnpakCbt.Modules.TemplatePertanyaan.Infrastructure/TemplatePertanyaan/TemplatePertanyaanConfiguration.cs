using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UnpakCbt.Modules.TemplatePertanyaan.Infrastructure.TemplatePertanyaan
{
    internal sealed class TemplatePertanyaanConfiguration : IEntityTypeConfiguration<Domain.TemplatePertanyaan.TemplatePertanyaan>
    {
        public void Configure(EntityTypeBuilder<Domain.TemplatePertanyaan.TemplatePertanyaan> builder)
        {
            //builder.HasOne<Category>().WithMany();
        }
    }
}
