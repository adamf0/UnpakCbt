using Microsoft.EntityFrameworkCore;
using UnpakCbt.Modules.Ujian.Domain.Ujian;
using UnpakCbt.Modules.Ujian.Infrastructure.Database;

namespace UnpakCbt.Modules.Ujian.Infrastructure.Ujian
{
    internal sealed class UjianRepository(UjianDbContext context) : IUjianRepository
    {
        public async Task<Domain.Ujian.Ujian> GetAsync(Guid Uuid, CancellationToken cancellationToken = default)
        {
            Domain.Ujian.Ujian ujian = await context.Ujian.SingleOrDefaultAsync(e => e.Uuid == Uuid, cancellationToken);
            return ujian;
        }

        public async Task<Domain.Ujian.Ujian> GetByNoRegWithJadwalAsync(string NoReg, int IdJadwalUjian, CancellationToken cancellationToken = default)
        {
            Domain.Ujian.Ujian ujian = await context.Ujian.SingleOrDefaultAsync(e => e.NoReg == NoReg && e.IdJadwalUjian == IdJadwalUjian, cancellationToken);
            return ujian;
        }

        public async Task DeleteAsync(Domain.Ujian.Ujian ujian)
        {
            context.Ujian.Remove(ujian);
        }

        public void Insert(Domain.Ujian.Ujian ujian)
        {
            context.Ujian.Add(ujian);
        }
    }
}
