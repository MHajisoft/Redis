using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using CacheService.Interfaces;

namespace CacheService.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var result = _memoryCache.Get<T>(key);
            return await Task.FromResult(result);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            _memoryCache.Set(key, value, expiration);
            await Task.CompletedTask;
        }

        public async Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            await Task.CompletedTask;
        }
    }
}