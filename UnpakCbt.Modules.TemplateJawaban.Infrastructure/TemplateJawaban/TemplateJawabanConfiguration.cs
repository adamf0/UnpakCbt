using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.TemplateJawaban.Infrastructure.TemplateJawaban
{
    internal sealed class TemplateJawabanConfiguration : IEntityTypeConfiguration<Domain.TemplateJawaban.TemplateJawaban>
    {
        public void Configure(EntityTypeBuilder<Domain.TemplateJawaban.TemplateJawaban> builder)
        {
            //builder.HasOne<Category>().WithMany();
        }
    }
}
