using Microsoft.EntityFrameworkCore;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;
using UnpakCbt.Modules.JadwalUjian.Infrastructure.Database;

namespace UnpakCbt.Modules.JadwalUjian.Infrastructure.JadwalUjian
{
    internal sealed class JadwalUjianRepository(JadwalUjianDbContext context) : IJadwalUjianRepository
    {
        public async Task<Domain.JadwalUjian.JadwalUjian> GetAsync(Guid Uuid, CancellationToken cancellationToken = default)
        {
            Domain.JadwalUjian.JadwalUjian bankSoal = await context.JadwalUjian.SingleOrDefaultAsync(e => e.Uuid == Uuid, cancellationToken);
            return bankSoal;
        }

        public async Task DeleteAsync(Domain.JadwalUjian.JadwalUjian bankSoal)
        {
            context.JadwalUjian.Remove(bankSoal);
        }

        public void Insert(Domain.JadwalUjian.JadwalUjian bankSoal)
        {
            context.JadwalUjian.Add(bankSoal);
        }
    }
}
