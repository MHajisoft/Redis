# Redis Data Types Supported

The CacheService exposes HTTP endpoints for the following Redis data types and operations.

Overview of supported types and operations

- String
  - Get/Set string values
  - Endpoints: `/api/cache/redis/string/{key}`, `/api/cache/redis/string`

- List
  - Push, Pop, Range, Length
  - Endpoints: `/api/cache/redis/list/{key}/push`, `/api/cache/redis/list/{key}/pop`, `/api/cache/redis/list/{key}/range`, `/api/cache/redis/list/{key}/length`

- Set
  - Add, Remove, Members, Contains, Size
  - Endpoints: `/api/cache/redis/set/{key}/add`, `/api/cache/redis/set/{key}/remove/{member}`, `/api/cache/redis/set/{key}/members`, `/api/cache/redis/set/{key}/contains/{member}`, `/api/cache/redis/set/{key}/size`

- Hash
  - Set field, Get field, Get all fields, Exists, Delete
  - Endpoints: `/api/cache/redis/hash/{key}/set`, `/api/cache/redis/hash/{key}/get/{field}`, `/api/cache/redis/hash/{key}/all`, `/api/cache/redis/hash/{key}/exists/{field}`, `/api/cache/redis/hash/{key}/delete/{field}`

- Sorted Set
  - Add (member with score), Range by score, Get score, Length
  - Endpoints: `/api/cache/redis/sortedset/{key}/add`, `/api/cache/redis/sortedset/{key}/range`, `/api/cache/redis/sortedset/{key}/score/{member}`, `/api/cache/redis/sortedset/{key}/length`

- Key operations / metadata
  - Exists, Expire, TTL
  - Endpoints: `/api/cache/redis/check/{key}`, `/api/cache/redis/{key}/expire`, `/api/cache/redis/{key}/ttl`

Notes about the refactor

- The controller implementation was refactored into multiple partial files to improve maintainability. This is an internal code organization change and does not affect the public API surface or routes.
- Partial files added under `dotnet-cache-solution/CacheService/Controllers/` include:
  - `CacheController.Memory.cs` (memory cache endpoints)
  - `CacheController.Redis.Basic.cs` (basic redis, strings, expiration)
  - `CacheController.Redis.Lists.cs` (list operations)
  - `CacheController.Redis.Sets.cs` (set operations)
  - `CacheController.Redis.Hash.cs` (hash operations)
  - `CacheController.Redis.SortedSet.cs` (sorted set operations)
  - `CacheController.Compare.cs` (compare memory vs redis)

Compatibility

- No breaking changes to routes or method signatures were made. Client integrations should continue to work without modifications.

If you want, I can:
- Add curl examples for each endpoint,
- Create a Postman collection or OpenAPI snippet,
- Or apply and commit these markdown updates to the repository now — tell me to proceed and I will commit the files.
