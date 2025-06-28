using Microsoft.EntityFrameworkCore;
using System;
using UnpakCbt.Modules.Ujian.Domain.Cbt;
using UnpakCbt.Modules.Ujian.Domain.LogEvent;
using UnpakCbt.Modules.Ujian.Infrastructure.Database;

namespace UnpakCbt.Modules.Ujian.Infrastructure.LogEvent
{
    internal sealed class LogRepository(UjianDbContext context) : ILogRepository
    {
        public async Task InsertAsync(Domain.LogEvent.Log log, CancellationToken cancellationToken = default)
        {
            await context.Log.AddAsync(log, cancellationToken);
        }
    }
}
