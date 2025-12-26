namespace CacheService.Interfaces
{
    /// <summary>
    /// Main interface for Redis cache service supporting all Redis data types
    /// </summary>
    public partial interface IRedisCacheService
    {
        /// <summary>
        /// Connect to Redis server
        /// </summary>
        Task<bool> ConnectAsync();

        /// <summary>
        /// Disconnect from Redis server
        /// </summary>
        Task DisconnectAsync();

        /// <summary>
        /// Check if connected to Redis
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Get database instance
        /// </summary>
        /// <param name="db">Database index (optional)</param>
        /// <returns>Redis database instance</returns>
        StackExchange.Redis.IDatabase GetDatabase(int? db = null);
    }
}