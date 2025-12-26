using Microsoft.AspNetCore.Mvc;
using CacheService.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
}
