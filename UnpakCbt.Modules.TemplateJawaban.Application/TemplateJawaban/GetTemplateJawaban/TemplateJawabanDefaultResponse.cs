
namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban
{
    public sealed record TemplateJawabanDefaultResponse
    {
        public string Id { get; set; }
        public string Uuid { get; set; }
        public string IdTemplateSoal { get; set; }
        public string JawabanText { get; set; }
        public string JawabanImg { get; set; }
    }
}
