using Microsoft.AspNetCore.Mvc;
using CacheService.Interfaces;

namespace CacheService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class CacheController(
        IMemoryCacheService memoryCacheService,
        IRedisCacheService redisCacheService,
        ILogger<CacheController> logger
    ) : ControllerBase;
}