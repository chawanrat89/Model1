using Microsoft.AspNetCore.Mvc;

namespace Model1.Models
{
    [ApiController]
    public class RestaurantController(MapDbContext db) : ControllerBase
    {
        [HttpGet("system/api/restaurant/list")]
        public ApiResult List()
        {
            return ApiResult.Ok();
        }
    }
}
