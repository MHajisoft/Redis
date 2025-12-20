using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;
using CacheService.Interfaces;

namespace CacheService.Services
{
    public partial class RedisCacheService : IRedisCacheService
    {
        public async Task<bool> HashSetAsync(string key, string field, string value)
        {
            var db = _redis.GetDatabase();
            return await db.HashSetAsync(key, field, value);
        }

        public async Task<string?> HashGetAsync(string key, string field)
        {
            var db = _redis.GetDatabase();
            var value = await db.HashGetAsync(key, field);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task<IDictionary<string, string>> HashGetAllAsync(string key)
        {
            var db = _redis.GetDatabase();
            var hashEntries = await db.HashGetAllAsync(key);
            var result = new Dictionary<string, string>();
            foreach (var entry in hashEntries)
            {
                result[entry.Name.ToString()] = entry.Value.ToString();
            }
            return result;
        }

        public async Task<bool> HashDeleteAsync(string key, string field)
        {
            var db = _redis.GetDatabase();
            return await db.HashDeleteAsync(key, field);
        }

        public async Task<bool> HashExistsAsync(string key, string field)
        {
            var db = _redis.GetDatabase();
            return await db.HashExistsAsync(key, field);
        }
    }
}