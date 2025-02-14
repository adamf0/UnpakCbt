
namespace UnpakCbt.Modules.TemplateJawaban.Application.TemplateJawaban.GetTemplateJawaban
{
    public sealed record TemplateJawabanResponse
    {
        public string Uuid { get; set; }
        public string UuidTemplateSoal { get; set; }
        public string JawabanText { get; set; }
        public string JawabanImg { get; set; }
    }
}
