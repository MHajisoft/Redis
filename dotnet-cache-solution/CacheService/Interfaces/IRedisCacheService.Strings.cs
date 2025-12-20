using System;
using System.Threading.Tasks;

namespace CacheService.Interfaces
{
    /// <summary>
    /// Partial interface for Redis string operations
    /// </summary>
    public partial interface IRedisCacheService
    {
        /// <summary>
        /// Get a string value from Redis
        /// </summary>
        /// <param name="key">Key to retrieve</param>
        /// <returns>String value or null if not found</returns>
        Task<string?> GetStringAsync(string key);

        /// <summary>
        /// Set a string value in Redis
        /// </summary>
        /// <param name="key">Key to set</param>
        /// <param name="value">String value to store</param>
        /// <param name="expiration">Optional expiration time</param>
        /// <returns>Awaitable task</returns>
        Task SetStringAsync(string key, string value, TimeSpan? expiration = null);
    }
}