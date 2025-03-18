namespace UnpakCbt.Modules.Ujian.Domain.Cbt
{
    public interface ICbtRepository
    {
        Task InsertAsync(IEnumerable<Cbt> cbts, CancellationToken cancellationToken = default);
        Task<Cbt?> GetAsync(Guid uuidUjian, Guid uuidTemplateSoal, string noReg, CancellationToken cancellationToken = default);
        Task DeleteAsync(int idUjian, CancellationToken cancellationToken = default);
    }
}
