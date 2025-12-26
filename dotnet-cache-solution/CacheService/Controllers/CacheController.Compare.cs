using Microsoft.AspNetCore.Mvc;

namespace CacheService.Controllers
{
    public partial class CacheController
    {
        // Compare both caches
        [HttpGet("compare/{key}")]
        public async Task<IActionResult> CompareCaches(string key)
        {
            try
            {
                var memoryValue = await memoryCacheService.GetAsync<string>(key);
                var redisValue = await redisCacheService.GetAsync<string>(key);

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
                logger.LogError(ex, "Error comparing caches");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}