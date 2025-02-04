using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnpakCbt.Modules.JadwalUjian.Domain.JadwalUjian;

namespace UnpakCbt.Modules.JadwalUjian.Infrastructure.JadwalUjian
{
    public class CounterRepository : ICounterRepository
    {
        private readonly IDatabase _redisDb;

        public CounterRepository(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public async Task<int> GetCounterAsync(string key)
        {
            var counterValue = await _redisDb.StringGetAsync(key);

            return (int)(counterValue.HasValue ? counterValue : 0);
        }

        public async Task<int> IncrementCounterAsync(string key, TimeSpan? timeout)
        {
            var newValue = await _redisDb.StringIncrementAsync(key);

            if (timeout.HasValue)
            {
                await SetExpirationAsync(key, timeout?? TimeSpan.Zero);
            }

            return (int) newValue;
        }

        public async Task<int> DecrementCounterAsync(string key, TimeSpan? timeout)
        {
            var newValue = await _redisDb.StringDecrementAsync(key);
            if (timeout != null)
            {
                await SetExpirationAsync(key, timeout ?? TimeSpan.Zero);
            }

            return (int) newValue;
        }

        public async Task ResetCounterAsync(string key, int value, TimeSpan? timeout)
        {
            await _redisDb.StringSetAsync(key, value);
            if (timeout != null)
            {
                await SetExpirationAsync(key, timeout ?? TimeSpan.Zero);
            }
        }

        public async Task<TimeSpan?> GetExpirationAsync(string key)
        {
            return await _redisDb.KeyTimeToLiveAsync(key);
        }
        public async Task<bool> DeleteKeyAsync(string key)
        {
            return await _redisDb.KeyDeleteAsync(key);
        }

        private async Task SetExpirationAsync(string key, TimeSpan timeToExpire)
        {
            if (timeToExpire > TimeSpan.Zero)
            {
                await _redisDb.KeyExpireAsync(key, timeToExpire);
            }
        }
    }
}
