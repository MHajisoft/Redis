using Microsoft.AspNetCore.Mvc;
using CacheService.Interfaces;
using CacheService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CacheService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class CacheController : ControllerBase
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
    }
}
