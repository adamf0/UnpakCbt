using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace UnpakCbt.Modules.Account.Infrastructure.Account
{
    internal sealed class AccountConfiguration : IEntityTypeConfiguration<Domain.Account.Account>
    {
        public void Configure(EntityTypeBuilder<Domain.Account.Account> builder)
        {
            //builder.HasOne<Category>().WithMany();
        }
    }
}
