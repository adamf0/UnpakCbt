using Microsoft.EntityFrameworkCore;
using UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban;
using UnpakCbt.Modules.TemplateJawaban.Infrastructure.Database;

namespace UnpakCbt.Modules.TemplateJawaban.Infrastructure.TemplateJawaban
{
    internal sealed class TemplateJawabanRepository(TemplateJawabanDbContext context) : ITemplateJawabanRepository
    {
        public async Task<Domain.TemplateJawaban.TemplateJawaban> GetAsync(Guid Uuid, CancellationToken cancellationToken = default)
        {
            Domain.TemplateJawaban.TemplateJawaban templateJawaban = await context.TemplateJawaban.SingleOrDefaultAsync(e => e.Uuid == Uuid, cancellationToken);
            return templateJawaban;
        }

        public async Task DeleteAsync(Domain.TemplateJawaban.TemplateJawaban templateJawaban)
        {
            context.TemplateJawaban.Remove(templateJawaban);
        }

        public void Insert(Domain.TemplateJawaban.TemplateJawaban templateJawaban)
        {
            context.TemplateJawaban.Add(templateJawaban);
        }
    }
}
