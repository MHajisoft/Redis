using Microsoft.AspNetCore.Mvc;
using CacheService.Models;

namespace CacheService.Controllers
{
    public partial class CacheController
    {
        // Redis Cache Endpoints (basic + strings + expiration)
        [HttpGet("redis/{key}")]
        public async Task<IActionResult> GetFromRedisCache(string key)
        {
            try
            {
                var value = await redisCacheService.GetAsync<string>(key);
                if (value == null)
                {
                    return NotFound($"Key '{key}' not found in Redis cache");
                }

                return Ok(new { Key = key, Value = value, CacheType = "Redis" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving from Redis cache");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("redis")]
        public async Task<IActionResult> SetToRedisCache([FromBody] CacheRequest request)
        {
            try
            {
                await redisCacheService.SetAsync(request.Key, request.Value, TimeSpan.FromMinutes(request.ExpirationInMinutes));
                return Ok(new { Message = $"Value set in Redis cache with key '{request.Key}'", Expiration = $"{request.ExpirationInMinutes} minutes" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error setting to Redis cache");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("redis/{key}")]
        public async Task<IActionResult> RemoveFromRedisCache(string key)
        {
            try
            {
                await redisCacheService.RemoveAsync(key);
                return Ok(new { Message = $"Key '{key}' removed from Redis cache" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing from Redis cache");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/check/{key}")]
        public async Task<IActionResult> CheckRedisKeyExists(string key)
        {
            try
            {
                var exists = await redisCacheService.ExistsAsync(key);
                return Ok(new { Key = key, Exists = exists });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking Redis key existence");
                return StatusCode(500, "Internal server error");
            }
        }

        // String operations
        [HttpGet("redis/string/{key}")]
        public async Task<IActionResult> GetRedisString(string key)
        {
            try
            {
                var value = await redisCacheService.GetStringAsync(key);
                if (value == null)
                {
                    return NotFound($"Key '{key}' not found in Redis cache");
                }

                return Ok(new { Key = key, Value = value, Type = "String" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving Redis string");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("redis/string")]
        public async Task<IActionResult> SetRedisString([FromBody] CacheRequest request)
        {
            try
            {
                await redisCacheService.SetStringAsync(request.Key, request.Value, TimeSpan.FromMinutes(request.ExpirationInMinutes));
                return Ok(new { Message = $"String value set in Redis with key '{request.Key}'", Expiration = $"{request.ExpirationInMinutes} minutes" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error setting Redis string");
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

                var expirationInMinutes = 10; // default

                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    if (prop.Name.Equals("expirationInMinutes", StringComparison.OrdinalIgnoreCase))
                        expirationInMinutes = prop.Value.GetInt32();
                }

                var success = await redisCacheService.ExpireAsync(key, TimeSpan.FromMinutes(expirationInMinutes));
                return Ok(new { Key = key, ExpirationInMinutes = expirationInMinutes, Success = success, Operation = "Expire" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error setting Redis key expiration");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/{key}/ttl")]
        public async Task<IActionResult> GetTimeToLive(string key)
        {
            try
            {
                var ttl = await redisCacheService.GetTimeToLiveAsync(key);
                return Ok(new { Key = key, TimeToLive = ttl?.TotalSeconds, Unit = "seconds" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting Redis TTL");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}