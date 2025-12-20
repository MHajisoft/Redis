using System;
using System.Threading.Tasks;

namespace CacheService.Interfaces
{
    public interface IMemoryCacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        Task RemoveAsync(string key);
    }
}