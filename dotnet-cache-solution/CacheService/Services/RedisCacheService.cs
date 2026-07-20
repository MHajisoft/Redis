using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using CacheService.Interfaces;

namespace CacheService.Services
{
    public partial class RedisCacheService(IDistributedCache distributedCache, IConnectionMultiplexer redis)
        : IRedisCacheService
    {
        public async Task<bool> ConnectAsync()
        {
            try
            {
                return redis.IsConnected;
            }
            catch
            {
                return false;
            }
        }

        public async Task DisconnectAsync()
        {
            await redis?.CloseAsync();
        }

        public bool IsConnected => redis?.IsConnected ?? false;

        public StackExchange.Redis.IDatabase GetDatabase(int? db = null)
        {
            return redis.GetDatabase(db ?? -1);
        }
    }
}