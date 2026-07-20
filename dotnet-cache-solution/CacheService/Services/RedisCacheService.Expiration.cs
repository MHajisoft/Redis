using CacheService.Interfaces;

namespace CacheService.Services
{
    public partial class RedisCacheService : IRedisCacheService
    {
        public async Task<bool> ExpireAsync(string key, TimeSpan expiration)
        {
            var db = redis.GetDatabase();
            return await db.KeyExpireAsync(key, expiration);
        }

        public async Task<TimeSpan?> GetTimeToLiveAsync(string key)
        {
            var db = redis.GetDatabase();
            var ttl = await db.KeyTimeToLiveAsync(key);
            return ttl ?? null;
        }
    }
}