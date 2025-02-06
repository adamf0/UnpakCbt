using Microsoft.EntityFrameworkCore;
using System.Threading;
using System;
using UnpakCbt.Modules.Ujian.Domain.Ujian;
using UnpakCbt.Modules.Ujian.Infrastructure.Database;

namespace UnpakCbt.Modules.Ujian.Infrastructure.Cbt
{
    internal sealed class CbtRepository(UjianDbContext context) : ICbtRepository
    {
        public async Task DeleteAsync(int idUjian, CancellationToken cancellationToken = default)
        {
            var cbts = await context.Cbt.Where(e => e.IdUjian == idUjian).ToListAsync(cancellationToken);
            context.Cbt.RemoveRange(cbts);
        }

        public async Task<Domain.Ujian.Cbt> GetAsync(Guid Uuid, CancellationToken cancellationToken = default)
        {
            Domain.Ujian.Cbt cbt = await context.Cbt.SingleOrDefaultAsync(e => e.Uuid == Uuid, cancellationToken);
            return cbt;
        }

        public async Task InsertAsync(IEnumerable<Domain.Ujian.Cbt> cbts, CancellationToken cancellationToken = default)
        {
            await context.Cbt.AddRangeAsync(cbts, cancellationToken);
        }
    }
}
