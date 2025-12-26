using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using CacheService.Interfaces;

namespace CacheService.Services
{
    public partial class RedisCacheService : IRedisCacheService
    {
        public async Task<T?> GetAsync<T>(string key)
        {
            var cachedValue = await distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cachedValue))
                return default(T);

            return JsonSerializer.Deserialize<T>(cachedValue);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await distributedCache.SetStringAsync(key, serializedValue, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            });
        }

        public async Task RemoveAsync(string key)
        {
            await distributedCache.RemoveAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            var db = redis.GetDatabase();
            return await db.KeyExistsAsync(key);
        }
    }
}