using Microsoft.AspNetCore.Mvc;
using CacheService.Models;

namespace CacheService.Controllers
{
    public partial class CacheController
    {
        // Set operations
        [HttpPost("redis/set/{key}/add")]
        public async Task<IActionResult> AddToSet(string key, [FromBody] CacheRequest request)
        {
            try
            {
                var added = await redisCacheService.SetAddAsync(key, request.Value);
                return Ok(new { Key = key, Member = request.Value, Added = added, Operation = "SetAdd" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding to Redis set");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/set/{key}/members")]
        public async Task<IActionResult> GetSetMembers(string key)
        {
            try
            {
                var members = await redisCacheService.SetMembersAsync(key);
                return Ok(new { Key = key, Members = members, Type = "Set" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting Redis set members");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/set/{key}/contains/{member}")]
        public async Task<IActionResult> CheckSetContains(string key, string member)
        {
            try
            {
                var contains = await redisCacheService.SetContainsAsync(key, member);
                return Ok(new { Key = key, Member = member, Contains = contains, Type = "Set" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error checking Redis set contains");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("redis/set/{key}/size")]
        public async Task<IActionResult> GetSetSize(string key)
        {
            try
            {
                var size = await redisCacheService.SetSizeAsync(key);
                return Ok(new { Key = key, Size = size, Type = "Set" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting Redis set size");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("redis/set/{key}/remove/{member}")]
        public async Task<IActionResult> RemoveFromSet(string key, string member)
        {
            try
            {
                var removed = await redisCacheService.SetRemoveAsync(key, member);
                return Ok(new { Key = key, Member = member, Removed = removed, Operation = "SetRemove" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error removing from Redis set");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}