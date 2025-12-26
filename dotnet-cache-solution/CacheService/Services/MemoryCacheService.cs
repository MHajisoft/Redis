using Microsoft.Extensions.Caching.Memory;
using CacheService.Interfaces;

namespace CacheService.Services
{
    public class MemoryCacheService(IMemoryCache memoryCache)
        : IMemoryCacheService
    {
        public async Task<T?> GetAsync<T>(string key)
        {
            var result = memoryCache.Get<T>(key);
            return await Task.FromResult(result);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            memoryCache.Set(key, value, expiration);
            await Task.CompletedTask;
        }

        public async Task RemoveAsync(string key)
        {
            memoryCache.Remove(key);
            await Task.CompletedTask;
        }
    }
}