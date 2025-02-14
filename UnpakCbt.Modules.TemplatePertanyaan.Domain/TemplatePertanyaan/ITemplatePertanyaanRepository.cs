namespace UnpakCbt.Modules.TemplatePertanyaan.Domain.TemplatePertanyaan
{
    public interface ITemplatePertanyaanRepository
    {
        void Insert(TemplatePertanyaan TemplatePertanyaan);
        Task<TemplatePertanyaan> GetAsync(Guid Uuid, CancellationToken cancellationToken = default);
        Task DeleteAsync(TemplatePertanyaan TemplatePertanyaan);
    }
}
