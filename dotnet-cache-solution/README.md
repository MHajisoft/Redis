# .NET 8 Cache Service Solution

This solution demonstrates the implementation of in-memory caching, Redis caching, and ZiggyCreatures.FusionCache integration in a .NET 8 application.

## Solution Structure

- **CacheService**: Main web API project containing cache implementations

## Features

1. **In-Memory Caching**
   - Uses `IMemoryCache` from `Microsoft.Extensions.Caching.Memory`
   - Provides basic cache operations (Get, Set, Remove)

2. **Redis Caching**
   - Uses `StackExchange.Redis` and `Microsoft.Extensions.Caching.StackExchangeRedis`
   - Provides cache operations with distributed caching capabilities
   - Includes support for all Redis data types (Strings, Lists, Sets, Hashes, Sorted Sets)
   - Key existence checking and expiration management

3. **FusionCache Integration**
   - Uses `ZiggyCreatures.FusionCache` with Redis backplane
   - Advanced caching features with built-in fail-safe mechanisms
   - Automatic serialization/deserialization
   - GetOrAdd pattern support
   - Cache statistics and bulk operations

## Packages Used

- `Microsoft.Extensions.Caching.Memory` (v8.0.0)
- `StackExchange.Redis` (v2.7.4)
- `Microsoft.Extensions.Caching.StackExchangeRedis` (v8.0.0)
- `ZiggyCreatures.FusionCache` (v1.0.0)
- `ZiggyCreatures.FusionCache.Protocol.StackExchangeRedis` (v1.0.0)

## API Endpoints

### Memory Cache Endpoints
- `GET /api/cache/memory/{key}` - Retrieve value from memory cache
- `POST /api/cache/memory` - Store value in memory cache
- `DELETE /api/cache/memory/{key}` - Remove value from memory cache

### Redis Cache Endpoints
- `GET /api/cache/redis/{key}` - Retrieve value from Redis cache
- `POST /api/cache/redis` - Store value in Redis cache
- `DELETE /api/cache/redis/{key}` - Remove value from Redis cache
- `GET /api/cache/redis/check/{key}` - Check if key exists in Redis
- `GET /api/cache/compare/{key}` - Compare values in both caches

### Redis Native Data Type Endpoints

#### String Operations
- `GET /api/cache/redis/string/{key}` - Get string value
- `POST /api/cache/redis/string` - Set string value with expiration

#### List Operations
- `POST /api/cache/redis/list/{key}/push` - Push item to list
- `GET /api/cache/redis/list/{key}/range` - Get list range
- `GET /api/cache/redis/list/{key}/pop` - Pop item from list
- `GET /api/cache/redis/list/{key}/length` - Get list length

#### Set Operations
- `POST /api/cache/redis/set/{key}/add` - Add member to set
- `GET /api/cache/redis/set/{key}/members` - Get all set members
- `GET /api/cache/redis/set/{key}/contains/{member}` - Check if set contains member
- `GET /api/cache/redis/set/{key}/size` - Get set size
- `DELETE /api/cache/redis/set/{key}/remove/{member}` - Remove member from set

#### Hash Operations
- `POST /api/cache/redis/hash/{key}/set` - Set hash field
- `GET /api/cache/redis/hash/{key}/get/{field}` - Get hash field
- `GET /api/cache/redis/hash/{key}/all` - Get all hash fields
- `GET /api/cache/redis/hash/{key}/exists/{field}` - Check if hash field exists
- `GET /api/cache/redis/hash/{key}/keys` - Get all hash keys
- `GET /api/cache/redis/hash/{key}/values` - Get all hash values
- `GET /api/cache/redis/hash/{key}/length` - Get hash length
- `DELETE /api/cache/redis/hash/{key}/delete/{field}` - Delete hash field

#### Sorted Set Operations
- `POST /api/cache/redis/sortedset/{key}/add` - Add member to sorted set with score
- `GET /api/cache/redis/sortedset/{key}/range` - Get sorted set range by rank
- `GET /api/cache/redis/sortedset/{key}/score` - Get member score
- `GET /api/cache/redis/sortedset/{key}/rank/{member}` - Get member rank
- `GET /api/cache/redis/sortedset/{key}/length` - Get sorted set length
- `DELETE /api/cache/redis/sortedset/{key}/remove/{member}` - Remove member from sorted set

#### Expiration Operations
- `POST /api/cache/redis/expire/{key}` - Set key expiration
- `GET /api/cache/redis/ttl/{key}` - Get key time-to-live

### FusionCache Endpoints
- `GET /api/cache/fusion/{key}` - Retrieve value from FusionCache
- `POST /api/cache/fusion` - Store value in FusionCache
- `DELETE /api/cache/fusion/{key}` - Remove value from FusionCache
- `GET /api/cache/fusion/check/{key}` - Check if key exists in FusionCache
- `POST /api/cache/fusion/getoradd/{key}` - Get or add value using factory pattern
- `GET /api/cache/fusion/count` - Get total count of items in FusionCache
- `POST /api/cache/fusion/clear` - Clear all items from FusionCache

### Comparison Endpoints
- `GET /api/cache/compare/all/{key}` - Compare values across all three caches (Memory, Redis, FusionCache)

### Request Body Format
```json
{
  "key": "my-key",
  "value": "my-value",
  "expirationInMinutes": 10
}
```

## Configuration

The Redis connection is configured in `appsettings.json` and defaults to `localhost:6379`.

## Usage

1. Start the application
2. Use the API endpoints to test caching functionality
3. Compare performance between in-memory, Redis, and FusionCache

## FusionCache Integration

The solution now integrates ZiggyCreatures.FusionCache with Redis as the distributed backplane. FusionCache provides:

- **Automatic Serialization**: No need to manually serialize/deserialize values
- **Fail-Safe Mechanisms**: Built-in resilience patterns for cache failures
- **GetOrAdd Pattern**: Atomic get-or-add operations with factory functions
- **Multi-Layer Caching**: Combines in-memory and distributed caching automatically
- **Backplane Support**: Redis backplane for cache invalidation across multiple instances

### Using FusionCache in Your Code

```csharp
public class MyService
{
    private readonly IFusionCacheService _fusionCache;

    public MyService(IFusionCacheService fusionCache)
    {
        _fusionCache = fusionCache;
    }

    // Simple get/set
    public async Task<string> GetData(string key)
    {
        return await _fusionCache.GetAsync<string>(key);
    }

    public async Task SaveData(string key, string value)
    {
        await _fusionCache.SetAsync(key, value, TimeSpan.FromMinutes(30));
    }

    // GetOrAdd pattern - atomically get or create cached value
    public async Task<string> GetOrGenerateData(string key)
    {
        return await _fusionCache.GetOrAddAsync(
            key,
            async () => await GenerateExpensiveData(),
            TimeSpan.FromHours(1)
        );
    }
}
```

## Running the Application

To run this application, you would typically:
1. Have .NET 8 SDK installed
2. Install Redis server (for Redis cache functionality)
3. Run `dotnet run` in the CacheService directory

Note: This solution includes proper dependency injection for all three cache services (Memory, Redis, and FusionCache), allowing easy testing and maintenance.