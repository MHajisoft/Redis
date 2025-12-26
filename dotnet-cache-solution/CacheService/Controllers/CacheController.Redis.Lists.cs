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
    }
}
