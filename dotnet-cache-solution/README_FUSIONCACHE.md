# FusionCache Service Implementation with ZiggyCreatures.FusionCache

## Overview

The solution now includes a `IFusionCacheService` interface and `FusionCacheService` implementation that provides advanced caching features using **ZiggyCreatures.FusionCache** with Redis as the distributed cache backend and backplane.

## What is FusionCache?

FusionCache is a modern, robust, and flexible caching library for .NET that provides:

- **Multi-layer caching**: Automatically combines in-memory and distributed caching
- **Built-in fail-safe mechanisms**: Continues working even when Redis is unavailable
- **Automatic serialization**: No manual JSON serialization needed
- **Backplane support**: Redis backplane for cache invalidation across multiple instances
- **GetOrAdd pattern**: Atomic operations to prevent cache stampede
- **Advanced features**: Cache warming, soft expiration, and more

## Architecture

### FusionCache with Redis Integration

The `FusionCacheService` integrates ZiggyCreatures.FusionCache with:
- **Redis Backplane**: For cache invalidation notifications across multiple application instances
- **Redis Distributed Cache**: As the primary distributed storage layer
- **System.Text.Json Serializer**: For automatic object serialization/deserialization

### Configuration

```csharp
// In Program.cs or Startup.cs
var options = new FusionCacheOptions
{
    DefaultDuration = TimeSpan.FromMinutes(30),
    EnableAutoWireupForCodeFirstApproach = false
};

_fusionCache = FusionCacheBuilder
    .Create()
    .WithOptions(options)
    .WithSerializer(new SystemTextJsonFusionCacheSerializer())
    .WithBackplane(new StackExchangeRedisFusionCacheBackplane(redis.GetSubscriber()))
    .WithDistributedCache(new StackExchangeRedisFusionCacheClient(redis.GetDatabase()))
    .Build();
```

## New Features

### IFusionCacheService Interface

Provides the following methods:

```csharp
Task<T?> GetAsync<T>(string key);
Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
Task RemoveAsync(string key);
Task<bool> ExistsAsync(string key);
Task<T?> GetOrAddAsync<T>(string key, Func<Task<T?>> factory, TimeSpan? expiration = null);
Task<long> GetCountAsync();
Task ClearAsync();
```

### Key Benefits of FusionCacheService

1. **Automatic Serialization**: Values are automatically serialized/deserialized using System.Text.Json
2. **GetOrAdd Pattern**: Atomic get-or-add operations prevent cache stampede
3. **Fail-Safe**: Built-in resilience patterns handle Redis outages gracefully
4. **Multi-Layer**: Combines L1 (in-memory) and L2 (Redis) caching automatically
5. **Backplane Support**: Automatic invalidation across multiple application instances
6. **Cache Statistics**: Ability to count items and clear the cache
7. **Simplified API**: Cleaner interface for common caching scenarios

## API Endpoints

### FusionCache Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/cache/fusion/{key}` | Get value from FusionCache |
| POST | `/api/cache/fusion` | Set value in FusionCache |
| DELETE | `/api/cache/fusion/{key}` | Remove value from FusionCache |
| GET | `/api/cache/fusion/check/{key}` | Check if key exists |
| POST | `/api/cache/fusion/getoradd/{key}` | Get or add value using factory |
| GET | `/api/cache/fusion/count` | Get cache item count |
| POST | `/api/cache/fusion/clear` | Clear all cache items |
| GET | `/api/cache/compare/all/{key}` | Compare all three caches (Memory, Redis, Fusion) |

## Usage Examples

### Using IFusionCacheService

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

    // Check existence
    public async Task<bool> HasData(string key)
    {
        return await _fusionCache.ExistsAsync(key);
    }

    // Get cache statistics
    public async Task<long> GetCacheSize()
    {
        return await _fusionCache.GetCountAsync();
    }

    // Clear cache
    public async Task ResetCache()
    {
        await _fusionCache.ClearAsync();
    }
}
```

### Dependency Injection Setup

In `Program.cs`:

```csharp
// Register services
builder.Services.AddScoped<IMemoryCacheService, MemoryCacheService>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddScoped<IFusionCacheService, FusionCacheService>();
```

## Comparison: When to Use Each Service

### Use IRedisCacheService when:
- You need direct access to Redis native data types (Lists, Sets, Hashes, Sorted Sets)
- You require fine-grained control over Redis operations
- You need atomic operations on specific Redis structures
- Performance is critical and you want minimal abstraction

### Use IFusionCacheService when:
- You want a simpler, higher-level caching API
- You need the GetOrAdd pattern for lazy loading
- You want built-in fail-safe mechanisms
- You need multi-layer caching (L1 + L2) automatically
- You're doing standard key-value caching with complex objects
- You need cache invalidation across multiple instances

### Use IMemoryCacheService when:
- You need ultra-fast, in-memory caching
- The cache doesn't need to be shared across instances
- You're caching temporary data that can be lost on restart

## Testing the Implementation

### Test FusionCache Endpoints

```bash
# Set a value
curl -X POST http://localhost:5000/api/cache/fusion \
  -H "Content-Type: application/json" \
  -d '{"key": "test-key", "value": "test-value", "expirationInMinutes": 30}'

# Get a value
curl http://localhost:5000/api/cache/fusion/test-key

# Check existence
curl http://localhost:5000/api/cache/fusion/check/test-key

# Get or add
curl -X POST http://localhost:5000/api/cache/fusion/getoradd/my-key \
  -H "Content-Type: application/json" \
  -d '{"key": "my-key", "value": "generated-value", "expirationInMinutes": 60}'

# Get count
curl http://localhost:5000/api/cache/fusion/count

# Clear cache
curl -X POST http://localhost:5000/api/cache/fusion/clear

# Compare all caches
curl http://localhost:5000/api/cache/compare/all/test-key
```

## Packages Required

Add these packages to your project:

```xml
<PackageReference Include="ZiggyCreatures.FusionCache" Version="1.0.0" />
<PackageReference Include="ZiggyCreatures.FusionCache.Protocol.StackExchangeRedis" Version="1.0.0" />
```

## Summary

This implementation provides:

1. **ZiggyCreatures.FusionCache Integration**: Modern caching library with advanced features
2. **Redis Backplane**: Distributed cache invalidation across multiple instances
3. **Automatic Serialization**: No manual JSON handling required
4. **Fail-Safe Mechanisms**: Graceful degradation when Redis is unavailable
5. **GetOrAdd Pattern**: Prevents cache stampede with atomic operations
6. **Multi-Layer Caching**: Combines in-memory and distributed caching automatically
7. **Clean API**: Simple interface for common caching scenarios
8. **Complete Documentation**: Usage examples and testing instructions
