using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnpakCbt.Modules.Ujian.Domain.Ujian
{
    public interface ICounterRepository
    {
        Task<int> GetCounterAsync(string key);
        Task<int> IncrementCounterAsync(string key, TimeSpan? timeout);
        Task<int> DecrementCounterAsync(string key, TimeSpan? timeout);
        Task ResetCounterAsync(string key, int value, TimeSpan? timeout);
        Task<TimeSpan?> GetExpirationAsync(string key);
        Task<bool> DeleteKeyAsync(string key);
    }
}
