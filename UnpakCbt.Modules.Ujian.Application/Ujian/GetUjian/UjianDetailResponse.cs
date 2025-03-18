namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian
{
    public sealed record UjianDetailResponse
    {
        public string Uuid { get; set; }
        public string NoReg { get; set; }
        public string UuidJadwalUjian { get; set; }
        public string Status { get; set; }
    }
}
