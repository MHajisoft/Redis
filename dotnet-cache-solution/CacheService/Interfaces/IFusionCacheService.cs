using System;
using System.Threading.Tasks;

namespace CacheService.Interfaces
{
    /// <summary>
    /// Main interface for FusionCache service supporting distributed caching with Redis backplane
    /// </summary>
    public partial interface IFusionCacheService
    {
        /// <summary>
        /// Get a value from FusionCache
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Value or default if not found</returns>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Set a value in FusionCache
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to store</param>
        /// <param name="expiration">Optional expiration time</param>
        /// <returns>Awaitable task</returns>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// Remove a key from FusionCache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Awaitable task</returns>
        Task RemoveAsync(string key);

        /// <summary>
        /// Check if a key exists in FusionCache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>True if key exists, false otherwise</returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Get or add a value to FusionCache using a factory function
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Factory function to produce the value if not cached</param>
        /// <param name="expiration">Optional expiration time</param>
        /// <returns>Cached or newly created value</returns>
        Task<T?> GetOrAddAsync<T>(string key, Func<Task<T?>> factory, TimeSpan? expiration = null);

        /// <summary>
        /// Get the current count of items in the cache
        /// </summary>
        /// <returns>Number of items in the cache</returns>
        Task<long> GetCountAsync();

        /// <summary>
        /// Clear all items from the cache
        /// </summary>
        /// <returns>Awaitable task</returns>
        Task ClearAsync();
    }
}
