namespace UnpakCbt.Modules.Ujian.Domain.LogEvent
{
    public interface ILogRepository
    {
        Task InsertAsync(Log log, CancellationToken cancellationToken = default);
    }
}
