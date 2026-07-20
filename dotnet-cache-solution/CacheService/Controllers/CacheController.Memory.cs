using Microsoft.AspNetCore.Mvc;
using CacheService.Models;

namespace CacheService.Controllers
{
    public partial class CacheController
    {
        // Memory Cache Endpoints
        [HttpGet("memory/{key}")]
        public async Task<IActionResult> GetFromMemoryCache(string key)
        {
            try
            {
                var value = await memoryCacheService.GetAsync<string>(key);
                if (value == null)
                {
                    return NotFound($"Key '{key}' not found in memory cache");
                }

                return Ok(new { Key = key, Value = value, CacheType = "Memory" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving from memory cache");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("memory")]
        public async Task<IActionResult> SetToMemoryCache([FromBody] CacheRequest request)
        {
            try
            {
                await memoryCacheService.SetAsync(request.Key, request.Value, TimeSpan.FromMinutes(request.ExpirationInMinutes));
                return Ok(new { Message = $"Value set in memory cache with key '{request.Key}'", Expiration = $"{request.ExpirationInMinutes} minutes" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error setting to memory cache");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("memory/{key}")]
        public async Task<IActionResult> RemoveFromMemoryCache(string key)
        {
            try
            {
                await memoryCacheService.RemoveAsync(key);
                return Ok(new { Message = $"Key '{key}' removed from memory cache" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing from memory cache");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}