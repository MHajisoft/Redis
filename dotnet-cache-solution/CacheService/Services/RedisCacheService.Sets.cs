using CacheService.Interfaces;

namespace CacheService.Services
{
    public partial class RedisCacheService : IRedisCacheService
    {
        public async Task<bool> SetAddAsync(string key, string member)
        {
            var db = redis.GetDatabase();
            return await db.SetAddAsync(key, member);
        }

        public async Task<bool> SetRemoveAsync(string key, string member)
        {
            var db = redis.GetDatabase();
            return await db.SetRemoveAsync(key, member);
        }

        public async Task<IEnumerable<string>> SetMembersAsync(string key)
        {
            var db = redis.GetDatabase();
            var members = await db.SetMembersAsync(key);
            var result = new List<string>();
            foreach (var member in members)
            {
                result.Add(member.ToString());
            }

            return result;
        }

        public async Task<bool> SetContainsAsync(string key, string member)
        {
            var db = redis.GetDatabase();
            return await db.SetContainsAsync(key, member);
        }

        public async Task<long> SetSizeAsync(string key)
        {
            var db = redis.GetDatabase();
            return await db.SetLengthAsync(key);
        }
    }
}