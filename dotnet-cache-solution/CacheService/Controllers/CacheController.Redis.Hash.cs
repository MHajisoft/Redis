using Microsoft.AspNetCore.Mvc;
using CacheService.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CacheService.Controllers
{
    public partial class CacheController
    {
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
    }
}
