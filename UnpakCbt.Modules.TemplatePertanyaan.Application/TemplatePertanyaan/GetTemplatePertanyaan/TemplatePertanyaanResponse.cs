namespace UnpakCbt.Modules.TemplatePertanyaan.Application.TemplatePertanyaan.GetTemplatePertanyaan
{
    public sealed record TemplatePertanyaanResponse
    {
        public string Uuid { get; set; }
        public string UuidBankSoal { get; set; }
        public string Tipe { get; set; }
        public string Pertanyaan { get; set; }
        public string Gambar { get; set; }
        public string UuidJawabanBenar { get; set; }
        public string Bobot { get; set; }
        public string State { get; set; }
        public string Trial { get; set; }
    }
}
