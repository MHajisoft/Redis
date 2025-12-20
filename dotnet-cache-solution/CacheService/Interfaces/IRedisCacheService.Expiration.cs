using System;
using System.Threading.Tasks;

namespace CacheService.Interfaces
{
    /// <summary>
    /// Partial interface for Redis expiration operations
    /// </summary>
    public partial interface IRedisCacheService
    {
        /// <summary>
        /// Set expiration time for a Redis key
        /// </summary>
        /// <param name="key">Key to set expiration for</param>
        /// <param name="expiration">Expiration time</param>
        /// <returns>True if expiration was set, false if key doesn't exist</returns>
        Task<bool> ExpireAsync(string key, TimeSpan expiration);

        /// <summary>
        /// Get the time to live for a Redis key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Time to live or null if key doesn't exist or has no expiration</returns>
        Task<TimeSpan?> GetTimeToLiveAsync(string key);
    }
}