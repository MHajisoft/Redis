namespace CacheService.Interfaces
{
    /// <summary>
    /// Partial interface for Redis set operations
    /// </summary>
    public partial interface IRedisCacheService
    {
        /// <summary>
        /// Add a member to a Redis set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="member">Member to add</param>
        /// <returns>True if member was added, false if it already existed</returns>
        Task<bool> SetAddAsync(string key, string member);

        /// <summary>
        /// Remove a member from a Redis set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="member">Member to remove</param>
        /// <returns>True if member was removed, false if it didn't exist</returns>
        Task<bool> SetRemoveAsync(string key, string member);

        /// <summary>
        /// Get all members of a Redis set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>All members in the set</returns>
        Task<IEnumerable<string>> SetMembersAsync(string key);

        /// <summary>
        /// Check if a member exists in a Redis set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="member">Member to check</param>
        /// <returns>True if member exists, false otherwise</returns>
        Task<bool> SetContainsAsync(string key, string member);

        /// <summary>
        /// Get the size of a Redis set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>Number of elements in the set</returns>
        Task<long> SetSizeAsync(string key);
    }
}