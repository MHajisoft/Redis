using Microsoft.AspNetCore.Mvc;
using CacheService.Interfaces;
using CacheService.Models;
using System.Collections.Generic;

namespace CacheService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger<CacheController> _logger;

        public CacheController(
            IMemoryCacheService memoryCacheService, 
            IRedisCacheService redisCacheService,
            ILogger<CacheController> logger)
        {
            _memoryCacheService = memoryCacheService;
            _redisCacheService = redisCacheService;
            _logger = logger;
        }

        // Memory Cache Endpoints
        [HttpGet("memory/{key}")]
        public async Task<IActionResult> GetFromMemoryCache(string key)
        {
            try
            {
                var value = await _memoryCacheService.GetAsync<string>(key);
                if (value == null)
                {
                    return NotFound($"Key '{key}' not found in memory cache");
                }

                return Ok(new { Key = key, Value = value, CacheType = "Memory" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving from memory cache");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("memory")]
        public async Task<IActionResult> SetToMemoryCache([FromBody] CacheRequest request)
        {
            try
            {
                await _memoryCacheService.SetAsync(request.Key, request.Value, TimeSpan.FromMinutes(request.ExpirationInMinutes));
                return Ok(new { Message = $"Value set in memory cache with key '{request.Key}'", Expiration = $"{request.ExpirationInMinutes} minutes" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting to memory cache");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("memory/{key}")]
        public async Task<IActionResult> RemoveFromMemoryCache(string key)
        {
            try
            {
                await _memoryCacheService.RemoveAsync(key);
                return Ok(new { Message = $"Key '{key}' removed from memory cache" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing from memory cache");
                return StatusCode(500, "Internal server error");
            }
        }

        // Redis Cache Endpoints
        [HttpGet("redis/{key}")]
        public async Task<IActionResult> GetFromRedisCache(string key)
        {
            try
            {
                var value = await _redisCacheService.GetAsync<string>(key);
                if (value == null)
                {
                    return NotFound($"Key '{key}' not found in Redis cache");
                }

                return Ok(new { Key = key, Value = value, CacheType = "Redis" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving from Redis cache");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("redis")]
        public async Task<IActionResult> SetToRedisCache([FromBody] CacheRequest request)
        {
            try
            {
                await _redisCacheService.SetAsync(request.Key, request.Value, TimeSpan.FromMinutes(request.ExpirationInMinutes));
                return Ok(new { Message = $"Value set in Redis cache with key '{request.Key}'", Expiration = $"{request.ExpirationInMinutes} minutes" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting to Redis cache");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("redis/{key}")]
        public async Task<IActionResult> RemoveFromRedisCache(string key)
        {
            try
            {
                await _redisCacheService.RemoveAsync(key);
                return Ok(new { Message = $"Key '{key}' removed from Redis cache" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing from Redis cache");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/check/{key}")]
        public async Task<IActionResult> CheckRedisKeyExists(string key)
        {
            try
            {
                var exists = await _redisCacheService.ExistsAsync(key);
                return Ok(new { Key = key, Exists = exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking Redis key existence");
                return StatusCode(500, "Internal server error");
            }
        }

        // Compare both caches
        [HttpGet("compare/{key}")]
        public async Task<IActionResult> CompareCaches(string key)
        {
            try
            {
                var memoryValue = await _memoryCacheService.GetAsync<string>(key);
                var redisValue = await _redisCacheService.GetAsync<string>(key);

                return Ok(new
                {
                    Key = key,
                    MemoryCacheValue = memoryValue,
                    RedisCacheValue = redisValue,
                    MemoryCacheExists = memoryValue != null,
                    RedisCacheExists = redisValue != null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error comparing caches");
                return StatusCode(500, "Internal server error");
            }
        }

        // String operations
        [HttpGet("redis/string/{key}")]
        public async Task<IActionResult> GetRedisString(string key)
        {
            try
            {
                var value = await _redisCacheService.GetStringAsync(key);
                if (value == null)
                {
                    return NotFound($"Key '{key}' not found in Redis cache");
                }

                return Ok(new { Key = key, Value = value, Type = "String" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Redis string");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("redis/string")]
        public async Task<IActionResult> SetRedisString([FromBody] CacheRequest request)
        {
            try
            {
                await _redisCacheService.SetStringAsync(request.Key, request.Value, TimeSpan.FromMinutes(request.ExpirationInMinutes));
                return Ok(new { Message = $"String value set in Redis with key '{request.Key}'", Expiration = $"{request.ExpirationInMinutes} minutes" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting Redis string");
                return StatusCode(500, "Internal server error");
            }
        }

        // List operations
        [HttpPost("redis/list/{key}/push")]
        public async Task<IActionResult> PushToList(string key, [FromBody] CacheRequest request)
        {
            try
            {
                var count = await _redisCacheService.ListPushAsync(key, request.Value);
                return Ok(new { Key = key, Value = request.Value, NewLength = count, Operation = "ListPush" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error pushing to Redis list");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/list/{key}/range")]
        public async Task<IActionResult> GetListRange(string key, [FromQuery] int start = 0, [FromQuery] int stop = -1)
        {
            try
            {
                var values = await _redisCacheService.ListRangeAsync(key, start, stop);
                return Ok(new { Key = key, Values = values, Type = "List" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Redis list range");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/list/{key}/pop")]
        public async Task<IActionResult> PopFromList(string key)
        {
            try
            {
                var value = await _redisCacheService.ListPopAsync(key);
                if (value == null)
                {
                    return NotFound($"List '{key}' is empty or does not exist");
                }

                return Ok(new { Key = key, Value = value, Operation = "ListPop" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error popping from Redis list");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/list/{key}/length")]
        public async Task<IActionResult> GetListLength(string key)
        {
            try
            {
                var length = await _redisCacheService.ListLengthAsync(key);
                return Ok(new { Key = key, Length = length, Type = "List" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Redis list length");
                return StatusCode(500, "Internal server error");
            }
        }

        // Set operations
        [HttpPost("redis/set/{key}/add")]
        public async Task<IActionResult> AddToSet(string key, [FromBody] CacheRequest request)
        {
            try
            {
                var added = await _redisCacheService.SetAddAsync(key, request.Value);
                return Ok(new { Key = key, Member = request.Value, Added = added, Operation = "SetAdd" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding to Redis set");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/set/{key}/members")]
        public async Task<IActionResult> GetSetMembers(string key)
        {
            try
            {
                var members = await _redisCacheService.SetMembersAsync(key);
                return Ok(new { Key = key, Members = members, Type = "Set" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Redis set members");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/set/{key}/contains/{member}")]
        public async Task<IActionResult> CheckSetContains(string key, string member)
        {
            try
            {
                var contains = await _redisCacheService.SetContainsAsync(key, member);
                return Ok(new { Key = key, Member = member, Contains = contains, Type = "Set" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking Redis set contains");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/set/{key}/size")]
        public async Task<IActionResult> GetSetSize(string key)
        {
            try
            {
                var size = await _redisCacheService.SetSizeAsync(key);
                return Ok(new { Key = key, Size = size, Type = "Set" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Redis set size");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("redis/set/{key}/remove/{member}")]
        public async Task<IActionResult> RemoveFromSet(string key, string member)
        {
            try
            {
                var removed = await _redisCacheService.SetRemoveAsync(key, member);
                return Ok(new { Key = key, Member = member, Removed = removed, Operation = "SetRemove" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing from Redis set");
                return StatusCode(500, "Internal server error");
            }
        }

        // Hash operations
        [HttpPost("redis/hash/{key}/set")]
        public async Task<IActionResult> SetHashField(string key, [FromBody] object request)
        {
            try
            {
                // Parse the request body to extract field and value
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var doc = System.Text.Json.JsonDocument.Parse(json);
                
                string field = "";
                string value = "";
                
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    if (prop.Name.Equals("field", StringComparison.OrdinalIgnoreCase))
                        field = prop.Value.GetString() ?? "";
                    else if (prop.Name.Equals("value", StringComparison.OrdinalIgnoreCase))
                        value = prop.Value.GetString() ?? "";
                }
                
                var success = await _redisCacheService.HashSetAsync(key, field, value);
                return Ok(new { Key = key, Field = field, Value = value, Success = success, Operation = "HashSet" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting Redis hash field");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/hash/{key}/get/{field}")]
        public async Task<IActionResult> GetHashField(string key, string field)
        {
            try
            {
                var value = await _redisCacheService.HashGetAsync(key, field);
                if (value == null)
                {
                    return NotFound($"Field '{field}' in hash '{key}' not found");
                }

                return Ok(new { Key = key, Field = field, Value = value, Type = "Hash" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Redis hash field");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/hash/{key}/all")]
        public async Task<IActionResult> GetAllHashFields(string key)
        {
            try
            {
                var hashData = await _redisCacheService.HashGetAllAsync(key);
                return Ok(new { Key = key, Fields = hashData, Type = "Hash" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all Redis hash fields");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/hash/{key}/exists/{field}")]
        public async Task<IActionResult> CheckHashFieldExists(string key, string field)
        {
            try
            {
                var exists = await _redisCacheService.HashExistsAsync(key, field);
                return Ok(new { Key = key, Field = field, Exists = exists, Type = "Hash" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking Redis hash field exists");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("redis/hash/{key}/delete/{field}")]
        public async Task<IActionResult> DeleteHashField(string key, string field)
        {
            try
            {
                var deleted = await _redisCacheService.HashDeleteAsync(key, field);
                return Ok(new { Key = key, Field = field, Deleted = deleted, Operation = "HashDelete" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Redis hash field");
                return StatusCode(500, "Internal server error");
            }
        }

        // Sorted Set operations
        [HttpPost("redis/sortedset/{key}/add")]
        public async Task<IActionResult> AddToSortedSet(string key, [FromBody] object request)
        {
            try
            {
                // Parse the request body to extract member and score
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var doc = System.Text.Json.JsonDocument.Parse(json);
                
                string member = "";
                double score = 0;
                
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    if (prop.Name.Equals("member", StringComparison.OrdinalIgnoreCase))
                        member = prop.Value.GetString() ?? "";
                    else if (prop.Name.Equals("score", StringComparison.OrdinalIgnoreCase))
                        score = prop.Value.GetDouble();
                }
                
                var added = await _redisCacheService.SortedSetAddAsync(key, member, score);
                return Ok(new { Key = key, Member = member, Score = score, Added = added, Operation = "SortedSetAdd" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding to Redis sorted set");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/sortedset/{key}/range")]
        public async Task<IActionResult> GetSortedSetRange(string key, [FromQuery] double min = double.NegativeInfinity, [FromQuery] double max = double.PositiveInfinity)
        {
            try
            {
                var members = await _redisCacheService.SortedSetRangeByScoreAsync(key, min, max);
                return Ok(new { Key = key, Members = members, Min = min, Max = max, Type = "SortedSet" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Redis sorted set range");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/sortedset/{key}/score/{member}")]
        public async Task<IActionResult> GetSortedSetScore(string key, string member)
        {
            try
            {
                var score = await _redisCacheService.SortedSetScoreAsync(key, member);
                if (score == null)
                {
                    return NotFound($"Member '{member}' in sorted set '{key}' not found");
                }

                return Ok(new { Key = key, Member = member, Score = score, Type = "SortedSet" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Redis sorted set score");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/sortedset/{key}/length")]
        public async Task<IActionResult> GetSortedSetLength(string key)
        {
            try
            {
                var length = await _redisCacheService.SortedSetLengthAsync(key);
                return Ok(new { Key = key, Length = length, Type = "SortedSet" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Redis sorted set length");
                return StatusCode(500, "Internal server error");
            }
        }

        // Expiration operations
        [HttpPost("redis/{key}/expire")]
        public async Task<IActionResult> SetExpiration(string key, [FromBody] object request)
        {
            try
            {
                // Parse the request body to extract expiration in minutes
                var json = System.Text.Json.JsonSerializer.Serialize(request);
                var doc = System.Text.Json.JsonDocument.Parse(json);
                
                int expirationInMinutes = 10; // default
                
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    if (prop.Name.Equals("expirationInMinutes", StringComparison.OrdinalIgnoreCase))
                        expirationInMinutes = prop.Value.GetInt32();
                }
                
                var success = await _redisCacheService.ExpireAsync(key, TimeSpan.FromMinutes(expirationInMinutes));
                return Ok(new { Key = key, ExpirationInMinutes = expirationInMinutes, Success = success, Operation = "Expire" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting Redis key expiration");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/{key}/ttl")]
        public async Task<IActionResult> GetTimeToLive(string key)
        {
            try
            {
                var ttl = await _redisCacheService.GetTimeToLiveAsync(key);
                return Ok(new { Key = key, TimeToLive = ttl?.TotalSeconds, Unit = "seconds" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Redis TTL");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}