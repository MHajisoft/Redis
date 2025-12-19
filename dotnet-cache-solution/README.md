# .NET 8 Cache Service Solution

This solution demonstrates the implementation of both in-memory caching and Redis caching in a .NET 8 application.

## Solution Structure

- **CacheService**: Main web API project containing cache implementations

## Features

1. **In-Memory Caching**
   - Uses `IMemoryCache` from `Microsoft.Extensions.Caching.Memory`
   - Provides basic cache operations (Get, Set, Remove)

2. **Redis Caching**
   - Uses `StackExchange.Redis` and `Microsoft.Extensions.Caching.StackExchangeRedis`
   - Provides cache operations with distributed caching capabilities
   - Includes key existence checking

## Packages Used

- `Microsoft.Extensions.Caching.Memory` (v8.0.0)
- `StackExchange.Redis` (v2.7.4)
- `Microsoft.Extensions.Caching.StackExchangeRedis` (v8.0.0)

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
3. Compare performance between in-memory and Redis caching

## Running the Application

To run this application, you would typically:
1. Have .NET 8 SDK installed
2. Install Redis server (for Redis cache functionality)
3. Run `dotnet run` in the CacheService directory

Note: This solution includes proper dependency injection for both cache services, allowing easy testing and maintenance.