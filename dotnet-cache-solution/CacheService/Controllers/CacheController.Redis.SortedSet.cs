using Microsoft.AspNetCore.Mvc;
using CacheService.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CacheService.Controllers
{
    public partial class CacheController
    {
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
    }
}
