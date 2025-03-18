using Microsoft.EntityFrameworkCore;
using System;
using UnpakCbt.Modules.Ujian.Domain.Cbt;
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

        public async Task<Domain.Cbt.Cbt?> GetAsync(Guid uuidUjian, Guid uuidTemplateSoal, string noReg, CancellationToken cancellationToken = default)
        {
            IQueryable<Domain.Cbt.Cbt?> query = from cbt in context.Cbt
                                                join ujian in context.Ujian on cbt.IdUjian equals ujian.Id
                                                join jadwalUjian in context.JadwalUjian on ujian.IdJadwalUjian equals jadwalUjian.Id
                                                join templatePertanyaan in context.TemplatePertanyaan on cbt.IdTemplateSoal equals templatePertanyaan.Id
                                                where ujian.Uuid == uuidUjian
                                                      && templatePertanyaan.Uuid == uuidTemplateSoal
                                                      && ujian.NoReg == noReg
                                                select cbt;

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task InsertAsync(IEnumerable<Domain.Cbt.Cbt> cbts, CancellationToken cancellationToken = default)
        {
            await context.Cbt.AddRangeAsync(cbts, cancellationToken);
        }
    }
}
