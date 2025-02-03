using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
