using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;
using CacheService.Interfaces;

namespace CacheService.Services
{
    public partial class RedisCacheService : IRedisCacheService
    {
        public async Task<long> ListPushAsync(string key, string value)
        {
            var db = _redis.GetDatabase();
            return await db.ListRightPushAsync(key, value);
        }

        public async Task<long> ListPushRangeAsync(string key, IEnumerable<string> values)
        {
            var db = _redis.GetDatabase();
            var redisValues = new List<RedisValue>();
            foreach (var value in values)
            {
                redisValues.Add(value);
            }
            return await db.ListRightPushAsync(key, redisValues.ToArray());
        }

        public async Task<IEnumerable<string>> ListRangeAsync(string key, int start = 0, int stop = -1)
        {
            var db = _redis.GetDatabase();
            var values = await db.ListRangeAsync(key, start, stop);
            var result = new List<string>();
            foreach (var value in values)
            {
                if (!value.IsNull)
                {
                    result.Add(value.ToString());
                }
            }
            return result;
        }

        public async Task<string?> ListPopAsync(string key)
        {
            var db = _redis.GetDatabase();
            var value = await db.ListLeftPopAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task<long> ListLengthAsync(string key)
        {
            var db = _redis.GetDatabase();
            return await db.ListLengthAsync(key);
        }
    }
}