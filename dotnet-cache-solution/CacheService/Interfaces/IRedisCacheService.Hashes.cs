namespace CacheService.Interfaces
{
    /// <summary>
    /// Partial interface for Redis hash operations
    /// </summary>
    public partial interface IRedisCacheService
    {
        /// <summary>
        /// Set a field-value pair in a Redis hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field name</param>
        /// <param name="value">Field value</param>
        /// <returns>True if field was added, false if it was updated</returns>
        Task<bool> HashSetAsync(string key, string field, string value);

        /// <summary>
        /// Get a value from a Redis hash field
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field name</param>
        /// <returns>Field value or null if not found</returns>
        Task<string?> HashGetAsync(string key, string field);

        /// <summary>
        /// Get all field-value pairs from a Redis hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>Dictionary of field-value pairs</returns>
        Task<IDictionary<string, string>> HashGetAllAsync(string key);

        /// <summary>
        /// Delete a field from a Redis hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field name to delete</param>
        /// <returns>True if field was deleted, false if it didn't exist</returns>
        Task<bool> HashDeleteAsync(string key, string field);

        /// <summary>
        /// Check if a field exists in a Redis hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field name to check</param>
        /// <returns>True if field exists, false otherwise</returns>
        Task<bool> HashExistsAsync(string key, string field);
    }
}