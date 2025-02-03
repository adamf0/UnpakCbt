namespace UnpakCbt.Modules.TemplatePertanyaan.PublicApi
{
    public interface ITemplatePertanyaanApi
    {
        Task<TemplatePertanyaanResponse?> GetAsync(Guid TemplatePertanyaanUuid, CancellationToken cancellationToken = default);
    }
}
