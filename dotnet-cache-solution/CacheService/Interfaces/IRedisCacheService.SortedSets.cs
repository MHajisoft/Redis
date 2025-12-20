using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CacheService.Interfaces
{
    /// <summary>
    /// Partial interface for Redis sorted set operations
    /// </summary>
    public partial interface IRedisCacheService
    {
        /// <summary>
        /// Add a member to a Redis sorted set with a score
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to add</param>
        /// <param name="score">Score for the member</param>
        /// <returns>True if member was added, false if it was updated</returns>
        Task<bool> SortedSetAddAsync(string key, string member, double score);

        /// <summary>
        /// Get members from a Redis sorted set within a score range
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score (default negative infinity)</param>
        /// <param name="max">Maximum score (default positive infinity)</param>
        /// <returns>Members within the score range</returns>
        Task<IEnumerable<string>> SortedSetRangeByScoreAsync(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity);

        /// <summary>
        /// Get the score of a member in a Redis sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to get score for</param>
        /// <returns>Score of the member or null if not found</returns>
        Task<double?> SortedSetScoreAsync(string key, string member);

        /// <summary>
        /// Get the length of a Redis sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <returns>Number of elements in the sorted set</returns>
        Task<long> SortedSetLengthAsync(string key);
    }
}