namespace UnpakCbt.Modules.Ujian.Application.Cbt.GetCbt
{
    public sealed record CbtResponse
    {
        public string uuid { get; set; }
        public string uuidUjian { get; set; }
        public string uuidTemplatePertanyaan { get; set; }
        public string pertanyaanText { get; set; }
        public string pertanyaanImg { get; set; }
        public string uuidTemplatePilihan { get; set; }
        public string jawabanText { get; set; }
        public string jawabanImg { get; set; }
        public string tipe { get; set; }
        public string uuidJadwalUjian { get; set; }
        public string uuidBankSoal { get; set; }
    }
}
