using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;
using CacheService.Interfaces;

namespace CacheService.Services
{
    public partial class RedisCacheService : IRedisCacheService
    {
        public async Task<bool> SortedSetAddAsync(string key, string member, double score)
        {
            var db = _redis.GetDatabase();
            return await db.SortedSetAddAsync(key, member, score);
        }

        public async Task<IEnumerable<string>> SortedSetRangeByScoreAsync(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity)
        {
            var db = _redis.GetDatabase();
            var members = await db.SortedSetRangeByScoreAsync(key, min, max);
            var result = new List<string>();
            foreach (var member in members)
            {
                result.Add(member.ToString());
            }
            return result;
        }

        public async Task<double?> SortedSetScoreAsync(string key, string member)
        {
            var db = _redis.GetDatabase();
            var score = await db.SortedSetScoreAsync(key, member);
            return score.HasValue ? score.Value : null;
        }

        public async Task<long> SortedSetLengthAsync(string key)
        {
            var db = _redis.GetDatabase();
            return await db.SortedSetLengthAsync(key);
        }
    }
}