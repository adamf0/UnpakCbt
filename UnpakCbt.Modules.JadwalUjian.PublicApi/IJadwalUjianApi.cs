namespace UnpakCbt.Modules.JadwalUjian.PublicApi
{
    public interface IJadwalUjianApi
    {
        Task<JadwalUjianResponse?> GetAsync(Guid JadwalUjianUuid, CancellationToken cancellationToken = default);
        Task<JadwalUjianResponse?> GetByIdAsync(int? id, CancellationToken cancellationToken = default);
    }
}
