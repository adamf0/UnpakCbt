using Microsoft.EntityFrameworkCore;
using UnpakCbt.Modules.Account.Infrastructure.Database;
using UnpakCbt.Modules.Account.Domain.Account;

namespace UnpakCbt.Modules.Account.Infrastructure.Account
{
    internal sealed class AccountRepository(AccountDbContext context) : IAccountRepository
    {
        public async Task<Domain.Account.Account> GetAsync(Guid Uuid, CancellationToken cancellationToken = default)
        {
            Domain.Account.Account Account = await context.Account.SingleOrDefaultAsync(e => e.Uuid == Uuid, cancellationToken);
            return Account;
        }

        public async Task DeleteAsync(Domain.Account.Account Account)
        {
            context.Account.Remove(Account);
        }

        public void Insert(Domain.Account.Account Account)
        {
            context.Account.Add(Account);
        }
    }
}
