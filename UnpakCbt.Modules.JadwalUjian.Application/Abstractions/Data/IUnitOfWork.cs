﻿
namespace UnpakCbt.Modules.JadwalUjian.Application.Abstractions.Data
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
