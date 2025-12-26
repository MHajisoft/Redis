namespace CacheService.Interfaces
{
    /// <summary>
    /// Partial interface for basic Redis cache operations
    /// </summary>
    public partial interface IRedisCacheService
    {
        /// <summary>
        /// Get a value from Redis cache
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Value or null if not found</returns>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Set a value in Redis cache
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to store</param>
        /// <param name="expiration">Expiration time</param>
        /// <returns>Awaitable task</returns>
        Task SetAsync<T>(string key, T value, TimeSpan expiration);

        /// <summary>
        /// Remove a key from Redis cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Awaitable task</returns>
        Task RemoveAsync(string key);

        /// <summary>
        /// Check if a key exists in Redis cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>True if key exists, false otherwise</returns>
        Task<bool> ExistsAsync(string key);
    }
}