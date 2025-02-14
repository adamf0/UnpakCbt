using Microsoft.EntityFrameworkCore;
using UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan;
using UnpakCbt.Modules.TemplatePertanyaan.Infrastructure.Database;

namespace UnpakCbt.Modules.TemplatePertanyaan.Infrastructure.TemplatePertanyaan
{
    internal sealed class TemplatePertanyaanRepository(TemplatePertanyaanDbContext context) : ITemplatePertanyaanRepository
    {
        public async Task<Domain.TemplatePertanyaan.TemplatePertanyaan> GetAsync(Guid Uuid, CancellationToken cancellationToken = default)
        {
            Domain.TemplatePertanyaan.TemplatePertanyaan templatePertanyaan = await context.TemplatePertanyaan.SingleOrDefaultAsync(e => e.Uuid == Uuid, cancellationToken);
            return templatePertanyaan;
        }

        public async Task DeleteAsync(Domain.TemplatePertanyaan.TemplatePertanyaan templatePertanyaan)
        {
            context.TemplatePertanyaan.Remove(templatePertanyaan);
        }

        public void Insert(Domain.TemplatePertanyaan.TemplatePertanyaan templatePertanyaan)
        {
            context.TemplatePertanyaan.Add(templatePertanyaan);
        }
    }
}
