namespace CacheService.Interfaces
{
    /// <summary>
    /// Partial interface for Redis list operations
    /// </summary>
    public partial interface IRedisCacheService
    {
        /// <summary>
        /// Push a value to the end of a Redis list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="value">Value to push</param>
        /// <returns>New length of the list</returns>
        Task<long> ListPushAsync(string key, string value);

        /// <summary>
        /// Push multiple values to the end of a Redis list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="values">Values to push</param>
        /// <returns>New length of the list</returns>
        Task<long> ListPushRangeAsync(string key, IEnumerable<string> values);

        /// <summary>
        /// Get a range of values from a Redis list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="start">Start index (default 0)</param>
        /// <param name="stop">Stop index (default -1 for end)</param>
        /// <returns>Range of values from the list</returns>
        Task<IEnumerable<string>> ListRangeAsync(string key, int start = 0, int stop = -1);

        /// <summary>
        /// Pop a value from the beginning of a Redis list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>Popped value or null if list is empty</returns>
        Task<string?> ListPopAsync(string key);

        /// <summary>
        /// Get the length of a Redis list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>Length of the list</returns>
        Task<long> ListLengthAsync(string key);
    }
}