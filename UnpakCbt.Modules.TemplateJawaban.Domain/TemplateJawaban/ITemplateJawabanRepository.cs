
namespace UnpakCbt.Modules.TemplateJawaban.Domain.TemplateJawaban
{
    public interface ITemplateJawabanRepository
    {
        void Insert(TemplateJawaban TemplateJawaban);
        Task<TemplateJawaban> GetAsync(Guid Uuid, CancellationToken cancellationToken = default);
        Task DeleteAsync(TemplateJawaban TemplateJawaban);
    }
}
