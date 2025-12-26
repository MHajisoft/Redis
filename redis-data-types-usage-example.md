# Redis Data Types — Usage Examples

This document shows example requests for using the Redis endpoints provided by the CacheService API. The service base route is `/api/cache` unless you have configured a different prefix.

Note about refactor

- The CacheController has been refactored into multiple partial files grouped by capability (Memory, Redis basic/strings/expiration, Lists, Sets, Hash, SortedSet, Compare). This is an internal code organization change; the public API routes, request shapes, and responses remain the same. The example endpoints below include the full route prefix (`/api/cache`) for clarity.
- For convenience, each endpoint below notes which partial controller file contains its implementation (for maintainers).

Base URL

- API base: `/api/cache`

Strings

- Get string value
  - GET `/api/cache/redis/string/{key}`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/string/my:key"
  - Response (200):
    {
      "key": "my:key",
      "value": "hello",
      "type": "String"
    }
  - Implemented in: CacheController.Redis.Basic.cs

- Set string value
  - POST `/api/cache/redis/string`
  - Body (JSON):
    {
      "key": "my:key",
      "value": "hello",
      "expirationInMinutes": 60
    }
  - Example curl:
    curl -sS -X POST "https://your-host/api/cache/redis/string" \
      -H "Content-Type: application/json" \
      -d '{"key":"my:key","value":"hello","expirationInMinutes":60}'
  - Response (200): { Message, Expiration }
  - Implemented in: CacheController.Redis.Basic.cs

Lists

- Push to list (right push)
  - POST `/api/cache/redis/list/{key}/push`
  - Body (JSON): { "value": "element-value" }
  - Example curl:
    curl -sS -X POST "https://your-host/api/cache/redis/list/my:list/push" \
      -H "Content-Type: application/json" -d '{"value":"element-value"}'
  - Response (200): { Key, Value, NewLength, Operation: "ListPush" }
  - Implemented in: CacheController.Redis.Lists.cs

- Get list range
  - GET `/api/cache/redis/list/{key}/range?start=0&stop=-1`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/list/my:list/range?start=0&stop=-1"
  - Response (200): { Key, Values, Type: "List" }
  - Implemented in: CacheController.Redis.Lists.cs

- Pop from list (pop)
  - GET `/api/cache/redis/list/{key}/pop`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/list/my:list/pop"
  - Response (200): { Key, Value, Operation: "ListPop" }
  - Implemented in: CacheController.Redis.Lists.cs

- Get list length
  - GET `/api/cache/redis/list/{key}/length`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/list/my:list/length"
  - Response (200): { Key, Length, Type: "List" }
  - Implemented in: CacheController.Redis.Lists.cs

Sets

- Add member to set
  - POST `/api/cache/redis/set/{key}/add`
  - Body (JSON): { "value": "member1" }
  - Example curl:
    curl -sS -X POST "https://your-host/api/cache/redis/set/my:set/add" \
      -H "Content-Type: application/json" -d '{"value":"member1"}'
  - Response (200): { Key, Member, Added, Operation: "SetAdd" }
  - Implemented in: CacheController.Redis.Sets.cs

- Get members
  - GET `/api/cache/redis/set/{key}/members`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/set/my:set/members"
  - Response (200): { Key, Members, Type: "Set" }
  - Implemented in: CacheController.Redis.Sets.cs

- Check membership
  - GET `/api/cache/redis/set/{key}/contains/{member}`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/set/my:set/contains/member1"
  - Response (200): { Key, Member, Contains, Type: "Set" }
  - Implemented in: CacheController.Redis.Sets.cs

- Remove member
  - DELETE `/api/cache/redis/set/{key}/remove/{member}`
  - Example curl:
    curl -sS -X DELETE "https://your-host/api/cache/redis/set/my:set/remove/member1"
  - Response (200): { Key, Member, Removed, Operation: "SetRemove" }
  - Implemented in: CacheController.Redis.Sets.cs

- Get set size
  - GET `/api/cache/redis/set/{key}/size`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/set/my:set/size"
  - Response (200): { Key, Size, Type: "Set" }
  - Implemented in: CacheController.Redis.Sets.cs

Hashes

- Set field
  - POST `/api/cache/redis/hash/{key}/set`
  - Body (JSON): { "field": "f1", "value": "v1" }
  - Example curl:
    curl -sS -X POST "https://your-host/api/cache/redis/hash/my:hash/set" \
      -H "Content-Type: application/json" -d '{"field":"f1","value":"v1"}'
  - Response (200): { Key, Field, Value, Success, Operation: "HashSet" }
  - Implemented in: CacheController.Redis.Hash.cs

- Get field
  - GET `/api/cache/redis/hash/{key}/get/{field}`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/hash/my:hash/get/f1"
  - Response (200): { Key, Field, Value, Type: "Hash" }
  - Implemented in: CacheController.Redis.Hash.cs

- Get all fields
  - GET `/api/cache/redis/hash/{key}/all`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/hash/my:hash/all"
  - Response (200): { Key, Fields, Type: "Hash" }
  - Implemented in: CacheController.Redis.Hash.cs

- Delete field
  - DELETE `/api/cache/redis/hash/{key}/delete/{field}`
  - Example curl:
    curl -sS -X DELETE "https://your-host/api/cache/redis/hash/my:hash/delete/f1"
  - Response (200): { Key, Field, Deleted, Operation: "HashDelete" }
  - Implemented in: CacheController.Redis.Hash.cs

Sorted Sets

- Add member with score
  - POST `/api/cache/redis/sortedset/{key}/add`
  - Body (JSON): { "member": "m1", "score": 1.5 }
  - Example curl:
    curl -sS -X POST "https://your-host/api/cache/redis/sortedset/my:zz/add" \
      -H "Content-Type: application/json" -d '{"member":"m1","score":1.5}'
  - Response (200): { Key, Member, Score, Added, Operation: "SortedSetAdd" }
  - Implemented in: CacheController.Redis.SortedSet.cs

- Range by score
  - GET `/api/cache/redis/sortedset/{key}/range?min=-inf&max=+inf`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/sortedset/my:zz/range?min=-inf&max=+inf"
  - Response (200): { Key, Members, Min, Max, Type: "SortedSet" }
  - Implemented in: CacheController.Redis.SortedSet.cs

- Get score of member
  - GET `/api/cache/redis/sortedset/{key}/score/{member}`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/sortedset/my:zz/score/m1"
  - Response (200): { Key, Member, Score, Type: "SortedSet" }
  - Implemented in: CacheController.Redis.SortedSet.cs

- Get sorted set length
  - GET `/api/cache/redis/sortedset/{key}/length`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/sortedset/my:zz/length"
  - Response (200): { Key, Length, Type: "SortedSet" }
  - Implemented in: CacheController.Redis.SortedSet.cs

Key management / metadata

- Check key exists
  - GET `/api/cache/redis/check/{key}`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/check/my:key"
  - Response (200): { Key, Exists }
  - Implemented in: CacheController.Redis.Basic.cs

- Set expiration
  - POST `/api/cache/redis/{key}/expire`
  - Body (JSON): { "expirationInMinutes": 10 }
  - Example curl:
    curl -sS -X POST "https://your-host/api/cache/redis/my:key/expire" \
      -H "Content-Type: application/json" -d '{"expirationInMinutes":10}'
  - Response (200): { Key, ExpirationInMinutes, Success, Operation: "Expire" }
  - Implemented in: CacheController.Redis.Basic.cs

- Get TTL (seconds)
  - GET `/api/cache/redis/{key}/ttl`
  - Example curl:
    curl -sS -X GET "https://your-host/api/cache/redis/my:key/ttl"
  - Response (200): { Key, TimeToLive, Unit: "seconds" }
  - Implemented in: CacheController.Redis.Basic.cs

Notes

- If you want runnable examples (Postman collection or ready curl scripts) added to this repo, open an issue or request an update and I can add them.
- If you notice any route differences in your deployed instance, check your routing/prefix configuration; these examples assume the default `/api/cache` prefix.
