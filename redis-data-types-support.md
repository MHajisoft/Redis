# Redis Data Types Support in Cache Service

This document describes the additional Redis data types support that has been added to the cache service.

## Overview

The Redis service has been enhanced to support all major Redis data types beyond the basic generic serialization approach. This allows for more efficient operations on Redis-specific data structures.

## Supported Redis Data Types

### 1. String Operations
- `GetStringAsync(string key)` - Get string value from Redis
- `SetStringAsync(string key, string value, TimeSpan? expiration = null)` - Set string value with optional expiration

### 2. List Operations
- `ListPushAsync(string key, string value)` - Push value to the end of a list
- `ListPushRangeAsync(string key, IEnumerable<string> values)` - Push multiple values to a list
- `ListRangeAsync(string key, int start = 0, int stop = -1)` - Get range of values from a list
- `ListPopAsync(string key)` - Pop and return first element from a list
- `ListLengthAsync(string key)` - Get length of a list

### 3. Set Operations
- `SetAddAsync(string key, string member)` - Add member to a set
- `SetRemoveAsync(string key, string member)` - Remove member from a set
- `SetMembersAsync(string key)` - Get all members of a set
- `SetContainsAsync(string key, string member)` - Check if member exists in a set
- `SetSizeAsync(string key)` - Get size of a set

### 4. Hash Operations
- `HashSetAsync(string key, string field, string value)` - Set field-value pair in a hash
- `HashGetAsync(string key, string field)` - Get value of a field in a hash
- `HashGetAllAsync(string key)` - Get all field-value pairs in a hash
- `HashDeleteAsync(string key, string field)` - Delete field from a hash
- `HashExistsAsync(string key, string field)` - Check if field exists in a hash

### 5. Sorted Set Operations
- `SortedSetAddAsync(string key, string member, double score)` - Add member with score to sorted set
- `SortedSetRangeByScoreAsync(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity)` - Get members in score range
- `SortedSetScoreAsync(string key, string member)` - Get score of a member
- `SortedSetLengthAsync(string key)` - Get length of sorted set

### 6. Expiration Operations
- `ExpireAsync(string key, TimeSpan expiration)` - Set expiration time for a key
- `GetTimeToLiveAsync(string key)` - Get remaining time to live for a key

## API Endpoints

### String Operations
- `GET /api/cache/redis/string/{key}` - Get string value
- `POST /api/cache/redis/string` - Set string value

### List Operations
- `POST /api/cache/redis/list/{key}/push` - Push to list
- `GET /api/cache/redis/list/{key}/range?start=0&stop=-1` - Get range from list
- `GET /api/cache/redis/list/{key}/pop` - Pop from list
- `GET /api/cache/redis/list/{key}/length` - Get list length

### Set Operations
- `POST /api/cache/redis/set/{key}/add` - Add to set
- `GET /api/cache/redis/set/{key}/members` - Get set members
- `GET /api/cache/redis/set/{key}/contains/{member}` - Check if member exists
- `GET /api/cache/redis/set/{key}/size` - Get set size
- `DELETE /api/cache/redis/set/{key}/remove/{member}` - Remove from set

### Hash Operations
- `POST /api/cache/redis/hash/{key}/set` - Set hash field (body: {"field": "field_name", "value": "value"})
- `GET /api/cache/redis/hash/{key}/get/{field}` - Get hash field
- `GET /api/cache/redis/hash/{key}/all` - Get all hash fields
- `GET /api/cache/redis/hash/{key}/exists/{field}` - Check if hash field exists
- `DELETE /api/cache/redis/hash/{key}/delete/{field}` - Delete hash field

### Sorted Set Operations
- `POST /api/cache/redis/sortedset/{key}/add` - Add to sorted set (body: {"member": "member_name", "score": 1.0})
- `GET /api/cache/redis/sortedset/{key}/range?min=-inf&max=+inf` - Get members by score range
- `GET /api/cache/redis/sortedset/{key}/score/{member}` - Get member score
- `GET /api/cache/redis/sortedset/{key}/length` - Get sorted set length

### Expiration Operations
- `POST /api/cache/redis/{key}/expire` - Set key expiration (body: {"expirationInMinutes": 10})
- `GET /api/cache/redis/{key}/ttl` - Get time to live

## Benefits

1. **Efficiency**: Operations on native Redis data types are more efficient than serializing complex objects
2. **Atomic Operations**: Redis provides atomic operations on its native data types
3. **Memory Optimization**: Native data types use memory more efficiently
4. **Rich Functionality**: Access to Redis-specific operations like sorted sets, which maintain order by score
5. **Better Performance**: Direct operations on Redis structures avoid serialization overhead

## Usage Examples

### Using List Operations
```csharp
// Push to a list
await redisCacheService.ListPushAsync("my-list", "item1");
await redisCacheService.ListPushAsync("my-list", "item2");

// Get all items in the list
var items = await redisCacheService.ListRangeAsync("my-list");
```

### Using Set Operations
```csharp
// Add members to a set
await redisCacheService.SetAddAsync("my-set", "member1");
await redisCacheService.SetAddAsync("my-set", "member2");

// Check if member exists
bool exists = await redisCacheService.SetContainsAsync("my-set", "member1");
```

### Using Hash Operations
```csharp
// Set fields in a hash
await redisCacheService.HashSetAsync("user:123", "name", "John Doe");
await redisCacheService.HashSetAsync("user:123", "email", "john@example.com");

// Get all fields from a hash
var userData = await redisCacheService.HashGetAllAsync("user:123");
```

### Using Sorted Set Operations
```csharp
// Add members with scores to a sorted set
await redisCacheService.SortedSetAddAsync("leaderboard", "player1", 100);
await redisCacheService.SortedSetAddAsync("leaderboard", "player2", 200);

// Get top players
var topPlayers = await redisCacheService.SortedSetRangeByScoreAsync("leaderboard", 150, double.PositiveInfinity);
```