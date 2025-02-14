namespace UnpakCbt.Modules.TemplatePertanyaan.PublicApi
{
    public sealed record TemplatePertanyaanResponse(string Id, string Uuid, int IdBankSoal, string Tipe, string? PertanyaanText, string? PertanyaanImg, int? JawabanBenar, int? Bobot, string? State);
}
