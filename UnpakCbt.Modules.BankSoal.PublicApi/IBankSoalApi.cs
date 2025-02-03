namespace UnpakCbt.Modules.BankSoal.PublicApi
{
    public interface IBankSoalApi
    {
        Task<BankSoalResponse?> GetAsync(Guid BankSoalUuid, CancellationToken cancellationToken = default);
    }
}
