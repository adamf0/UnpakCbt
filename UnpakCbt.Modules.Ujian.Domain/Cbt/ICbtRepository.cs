namespace UnpakCbt.Modules.Ujian.Domain.Ujian
{
    public interface ICbtRepository
    {
        Task InsertAsync(IEnumerable<Cbt> cbts, CancellationToken cancellationToken = default);
        Task<Cbt?> GetAsync(Guid uuid, CancellationToken cancellationToken = default);
        Task DeleteAsync(int idUjian, CancellationToken cancellationToken = default);
    }
}
