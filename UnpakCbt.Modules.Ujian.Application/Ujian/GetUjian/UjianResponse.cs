namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian
{
    public sealed record UjianResponse
    {
        public string Uuid { get; set; }
        public string NoReg { get; set; } = default!;
        public int JadwalUjian { get; set; }
    }
}
