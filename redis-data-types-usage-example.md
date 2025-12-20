# Redis Data Types Usage Examples

This document provides practical examples of how to use the newly added Redis data type operations in the cache service.

## HTTP API Usage Examples

### String Operations

#### Get String Value
```
GET /api/cache/redis/string/mykey
```

#### Set String Value
```
POST /api/cache/redis/string
Content-Type: application/json

{
  "key": "mykey",
  "value": "Hello Redis",
  "expirationInMinutes": 30
}
```

### List Operations

#### Push to List
```
POST /api/cache/redis/list/mystack/push
Content-Type: application/json

{
  "value": "item1"
}
```

#### Get List Range
```
GET /api/cache/redis/list/mystack/range?start=0&stop=-1
```

#### Get List Length
```
GET /api/cache/redis/list/mystack/length
```

#### Pop from List
```
GET /api/cache/redis/list/mystack/pop
```

### Set Operations

#### Add to Set
```
POST /api/cache/redis/set/myset/add
Content-Type: application/json

{
  "value": "member1"
}
```

#### Get Set Members
```
GET /api/cache/redis/set/myset/members
```

#### Check if Set Contains Member
```
GET /api/cache/redis/set/myset/contains/member1
```

#### Get Set Size
```
GET /api/cache/redis/set/myset/size
```

#### Remove from Set
```
DELETE /api/cache/redis/set/myset/remove/member1
```

### Hash Operations

#### Set Hash Field
```
POST /api/cache/redis/hash/user:123/set
Content-Type: application/json

{
  "field": "name",
  "value": "John Doe"
}
```

#### Get Hash Field
```
GET /api/cache/redis/hash/user:123/get/name
```

#### Get All Hash Fields
```
GET /api/cache/redis/hash/user:123/all
```

#### Check if Hash Field Exists
```
GET /api/cache/redis/hash/user:123/exists/name
```

#### Delete Hash Field
```
DELETE /api/cache/redis/hash/user:123/delete/name
```

### Sorted Set Operations

#### Add to Sorted Set
```
POST /api/cache/redis/sortedset/leaderboard/add
Content-Type: application/json

{
  "member": "player1",
  "score": 150
}
```

#### Get Members by Score Range
```
GET /api/cache/redis/sortedset/leaderboard/range?min=100&max=200
```

#### Get Member Score
```
GET /api/cache/redis/sortedset/leaderboard/score/player1
```

#### Get Sorted Set Length
```
GET /api/cache/redis/sortedset/leaderboard/length
```

### Expiration Operations

#### Set Key Expiration
```
POST /api/cache/redis/mykey/expire
Content-Type: application/json

{
  "expirationInMinutes": 15
}
```

#### Get Time to Live
```
GET /api/cache/redis/mykey/ttl
```

## C# Code Usage Examples

### Using the Redis Cache Service Directly

```csharp
using CacheService.Interfaces;

public class ExampleService
{
    private readonly IRedisCacheService _redisCacheService;

    public ExampleService(IRedisCacheService redisCacheService)
    {
        _redisCacheService = redisCacheService;
    }

    public async Task ExampleUsage()
    {
        // String operations
        await _redisCacheService.SetStringAsync("greeting", "Hello World", TimeSpan.FromMinutes(30));
        var greeting = await _redisCacheService.GetStringAsync("greeting");

        // List operations
        await _redisCacheService.ListPushAsync("tasks", "task1");
        await _redisCacheService.ListPushAsync("tasks", "task2");
        var tasks = await _redisCacheService.ListRangeAsync("tasks");
        var listLength = await _redisCacheService.ListLengthAsync("tasks");

        // Set operations
        await _redisCacheService.SetAddAsync("unique_visitors", "user123");
        await _redisCacheService.SetAddAsync("unique_visitors", "user456");
        var allVisitors = await _redisCacheService.SetMembersAsync("unique_visitors");
        var isVisitor = await _redisCacheService.SetContainsAsync("unique_visitors", "user123");
        var setSize = await _redisCacheService.SetSizeAsync("unique_visitors");

        // Hash operations
        await _redisCacheService.HashSetAsync("user:123", "name", "John Doe");
        await _redisCacheService.HashSetAsync("user:123", "email", "john@example.com");
        var userName = await _redisCacheService.HashGetAsync("user:123", "name");
        var userData = await _redisCacheService.HashGetAllAsync("user:123");
        var fieldExists = await _redisCacheService.HashExistsAsync("user:123", "name");

        // Sorted Set operations
        await _redisCacheService.SortedSetAddAsync("leaderboard", "player1", 100);
        await _redisCacheService.SortedSetAddAsync("leaderboard", "player2", 200);
        var topPlayers = await _redisCacheService.SortedSetRangeByScoreAsync("leaderboard", 150, double.PositiveInfinity);
        var playerScore = await _redisCacheService.SortedSetScoreAsync("leaderboard", "player1");

        // Expiration operations
        await _redisCacheService.ExpireAsync("temp_data", TimeSpan.FromMinutes(5));
        var ttl = await _redisCacheService.GetTimeToLiveAsync("temp_data");
    }
}
```

## Common Use Cases

### 1. Session Management with Hashes
```csharp
// Store user session data in a hash
await redisCacheService.HashSetAsync("session:abc123", "user_id", "123");
await redisCacheService.HashSetAsync("session:abc123", "username", "john_doe");
await redisCacheService.HashSetAsync("session:abc123", "login_time", DateTime.UtcNow.ToString());
```

### 2. Leaderboard with Sorted Sets
```csharp
// Create a game leaderboard
await redisCacheService.SortedSetAddAsync("game:leaderboard", "player1", 1250);
await redisCacheService.SortedSetAddAsync("game:leaderboard", "player2", 1500);
await redisCacheService.SortedSetAddAsync("game:leaderboard", "player3", 980);

// Get top 10 players
var topPlayers = await redisCacheService.SortedSetRangeByScoreAsync("game:leaderboard", 
    double.NegativeInfinity, double.PositiveInfinity);
```

### 3. Recent Activity with Lists
```csharp
// Maintain a list of recent activities
await redisCacheService.ListPushAsync("recent_activities", "user123 logged in");
await redisCacheService.ListPushAsync("recent_activities", "user123 updated profile");

// Keep only the last 100 activities
if (await redisCacheService.ListLengthAsync("recent_activities") > 100)
{
    // Trim the list to last 100 items
    var allActivities = await redisCacheService.ListRangeAsync("recent_activities", 0, 99);
    // (In a real implementation, you'd use LTRIM here)
}
```

### 4. Unique Visitors with Sets
```csharp
// Track unique visitors to a website
await redisCacheService.SetAddAsync("unique_visitors_today", "ip_192.168.1.1");
await redisCacheService.SetAddAsync("unique_visitors_today", "ip_192.168.1.2");

// Get total unique visitors
var uniqueCount = await redisCacheService.SetSizeAsync("unique_visitors_today");
```

These examples demonstrate how the enhanced Redis service can be used for various practical applications, taking advantage of Redis's native data structures for improved performance and functionality.