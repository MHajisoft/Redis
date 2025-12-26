using Microsoft.AspNetCore.Mvc;
using CacheService.Interfaces;
using CacheService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
    }
}
