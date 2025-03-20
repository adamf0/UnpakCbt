namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian
{
    public sealed record UjianResponse
    {
        public string Uuid { get; set; }
        public string NoReg { get; set; } = default!;
        public string UuidJadwalUjian { get; set; }
        public string Status { get; set; }
    }
}
