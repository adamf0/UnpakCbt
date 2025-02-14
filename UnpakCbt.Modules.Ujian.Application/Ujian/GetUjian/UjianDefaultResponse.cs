namespace UnpakCbt.Modules.Ujian.Application.Ujian.GetUjian
{
    public sealed record UjianDefaultResponse
    {
        public string Id { get; set; }
        public string Uuid { get; set; }
        public string NoReg { get; set; } = default!;
        public int JadwalUjian { get; set; }
    }
}
