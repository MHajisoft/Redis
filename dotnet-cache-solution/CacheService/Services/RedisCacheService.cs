using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using CacheService.Interfaces;

namespace CacheService.Services
{
    public partial class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IDistributedCache distributedCache, IConnectionMultiplexer redis)
        {
            _distributedCache = distributedCache;
            _redis = redis;
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                return _redis.IsConnected;
            }
            catch
            {
                return false;
            }
        }

        public async Task DisconnectAsync()
        {
            _redis?.Close();
        }

        public bool IsConnected => _redis?.IsConnected ?? false;

        public StackExchange.Redis.IDatabase GetDatabase(int? db = null)
        {
            return _redis.GetDatabase(db ?? -1);
        }
    }
}