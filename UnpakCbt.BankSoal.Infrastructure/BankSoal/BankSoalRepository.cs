using Microsoft.EntityFrameworkCore;
using UnpakCbt.Modules.BankSoal.Domain.BankSoal;
using UnpakCbt.Modules.BankSoal.Infrastructure.Database;

namespace UnpakCbt.Modules.BankSoal.Infrastructure.BankSoal
{
    internal sealed class BankSoalRepository(BankSoalDbContext context) : IBankSoalRepository
    {
        public async Task<Domain.BankSoal.BankSoal> GetAsync(Guid Uuid, CancellationToken cancellationToken = default)
        {
            Domain.BankSoal.BankSoal bankSoal = await context.BankSoal.SingleOrDefaultAsync(e => e.Uuid == Uuid, cancellationToken);
            return bankSoal;
        }

        public async Task DeleteAsync(Domain.BankSoal.BankSoal bankSoal)
        {
            context.BankSoal.Remove(bankSoal);
        }

        public void Insert(Domain.BankSoal.BankSoal bankSoal)
        {
            context.BankSoal.Add(bankSoal);
        }
    }
}
