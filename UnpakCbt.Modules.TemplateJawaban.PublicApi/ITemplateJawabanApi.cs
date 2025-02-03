namespace UnpakCbt.Modules.TemplateJawaban.PublicApi
{
    public interface ITemplateJawabanApi
    {
        Task<TemplateJawabanResponse?> GetAsync(Guid TemplateJawabanUuid, CancellationToken cancellationToken = default);
    }
}
