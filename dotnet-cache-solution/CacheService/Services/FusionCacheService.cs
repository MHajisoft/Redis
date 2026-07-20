using System;
using System.Threading.Tasks;
using CacheService.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Backplane.StackExchangeRedis;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace CacheService.Services
{
    /// <summary>
    /// FusionCache service implementation using ZiggyCreatures.FusionCache with Redis backplane
    /// Provides advanced caching features with built-in fail-safe mechanisms, auto-wiring, and more
    /// </summary>
    public class FusionCacheService : IFusionCacheService
    {
        private readonly IFusionCache _fusionCache;
        private readonly IConnectionMultiplexer _redis;

        public FusionCacheService(IConnectionMultiplexer redis, ILogger<FusionCacheService>? logger = null)
        {
            _redis = redis;
            
            // Configure FusionCache with Redis backplane
            var options = new FusionCacheOptions
            {
                DefaultDuration = TimeSpan.FromMinutes(30),
                EnableAutoWireupForCodeFirstApproach = false
            };

            // Create FusionCache instance with Redis backplane for distributed scenarios
            _fusionCache = FusionCacheBuilder
                .Create()
                .WithOptions(options)
                .WithSerializer(new SystemTextJsonFusionCacheSerializer())
                .WithBackplane(new StackExchangeRedisFusionCacheBackplane(redis.GetSubscriber()))
                .WithDistributedCache(new StackExchangeRedisFusionCacheClient(redis.GetDatabase()))
                .Build();

            if (logger != null)
            {
                _fusionCache.SetLogger(logger);
            }
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                return await _fusionCache.GetAsync<T>(key);
            }
            catch
            {
                return default(T);
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var duration = expiration ?? TimeSpan.FromMinutes(30);
            await _fusionCache.SetAsync(key, value, duration);
        }

        public async Task RemoveAsync(string key)
        {
            await _fusionCache.RemoveAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _fusionCache.ExistsAsync(key);
        }

        public async Task<T?> GetOrAddAsync<T>(string key, Func<Task<T?>> factory, TimeSpan? expiration = null)
        {
            var duration = expiration ?? TimeSpan.FromMinutes(30);
            
            return await _fusionCache.GetOrAddAsync(
                key,
                async (_) => await factory(),
                duration
            );
        }

        public async Task<long> GetCountAsync()
        {
            var db = _redis.GetDatabase();
            var endpoints = _redis.GetEndPoints();
            long count = 0;

            foreach (var endpoint in endpoints)
            {
                var server = _redis.GetServer(endpoint);
                var keys = server.Keys(pattern: "*");
                await foreach (var _ in keys)
                {
                    count++;
                }
            }

            return count;
        }

        public async Task ClearAsync()
        {
            var db = _redis.GetDatabase();
            var endpoints = _redis.GetEndPoints();

            foreach (var endpoint in endpoints)
            {
                var server = _redis.GetServer(endpoint);
                var keys = server.Keys(pattern: "*");
                
                await foreach (var key in keys)
                {
                    await db.KeyDeleteAsync(key);
                }
            }
        }
    }
}
