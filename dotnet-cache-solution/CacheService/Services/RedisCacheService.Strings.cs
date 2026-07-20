using CacheService.Interfaces;

namespace CacheService.Services
{
    public partial class RedisCacheService : IRedisCacheService
    {
        public async Task<string?> GetStringAsync(string key)
        {
            var db = redis.GetDatabase();
            var value = await db.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task SetStringAsync(string key, string value, TimeSpan? expiration = null)
        {
            var db = redis.GetDatabase();
            await db.StringSetAsync(key, value, expiration);
        }
    }
}