
namespace UnpakCbt.Modules.Account.Domain.Account
{
    public interface IAccountRepository
    {
        void Insert(Account Account);
        Task<Account> GetAsync(Guid Uuid, CancellationToken cancellationToken = default);
        Task DeleteAsync(Account Account);
        Task<int> CountByUsernameAsync(string Username, CancellationToken cancellationToken = default);
    }
}
