
namespace UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian
{
    public interface IJadwalUjianRepository
    {
        void Insert(JadwalUjian JadwalUjian);
        Task<JadwalUjian> GetAsync(Guid Uuid, CancellationToken cancellationToken = default);
        Task DeleteAsync(JadwalUjian JadwalUjian);
    }
}
