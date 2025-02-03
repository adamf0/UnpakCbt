using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace UnpakCbt.Modules.BankSoal.Infrastructure.BankSoal
{
    internal sealed class BankSoalConfiguration : IEntityTypeConfiguration<Domain.BankSoal.BankSoal>
    {
        public void Configure(EntityTypeBuilder<Domain.BankSoal.BankSoal> builder)
        {
            //builder.HasOne<Category>().WithMany();
        }
    }
}
