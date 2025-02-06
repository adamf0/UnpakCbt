namespace UnpakCbt.Modules.TemplatePertanyaan.PublicApi
{
    public interface ITemplatePertanyaanApi
    {
        Task<TemplatePertanyaanResponse?> GetAsync(Guid TemplatePertanyaanUuid, CancellationToken cancellationToken = default);
        Task<List<TemplatePertanyaanResponse>> GetAllTemplatePertanyaanByBankSoal(int IdBankSoal, CancellationToken cancellationToken = default);
    }
}
