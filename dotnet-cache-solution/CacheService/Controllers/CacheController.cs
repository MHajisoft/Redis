using Microsoft.AspNetCore.Mvc;

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
    }

    public class CacheRequest
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int ExpirationInMinutes { get; set; } = 10;
    }
}