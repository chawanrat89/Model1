using Microsoft.AspNetCore.Mvc;

namespace Model1.Models
{
    [ApiController]
    public class RestaurantController(MapDbContext db) : ControllerBase
    {
        [HttpGet("system/api/restaurant/list")]
        public ApiResult List(int pageNum = 1,
            int pageSize = 10
            ,string?restaurantName = null
            ,string?restaurantType = null)
        {
            var q = db.Restaurants.AsQueryable(); //ใช้เชื่อมต่อกับตารางRestaurants 
            if (!string.IsNullOrEmpty(restaurantName))  //ยังไม่ได้กรองชื่อ
            q = q.Where(x => x.RestaurantName!.Contains(restaurantName)); //เงื่อนไขที่1
            if (!string.IsNullOrEmpty(restaurantType))
                q = q.Where(x => x.RestaurantType == restaurantType); //กรองชื่อ

            int total = q.Count();
            var rows = q.OrderBy(x => x.RestaurantId)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(x => new
                {
                    x.RestaurantId,
                    x.RestaurantName,
                    x.Address,
                    x.Phone,
                    rating = x.Rating.ToString(),
                    x.PriceRange,
                    x.RestaurantType,
                    x.ImageUrl,
                    x.Description,
                    distance = x.Distance.ToString(),
                    businessType = "restaurant"});
            return ApiResult.Ok(new { total, rows});
        }

        [HttpGet("system/api/restaurant/{restaurantId}")]
            public ApiResult Detail(int restaurantId)
            {
                var x = db.Restaurants.FirstOrDefault(r => r.RestaurantId == restaurantId);
                if (x == null)
                    return ApiResult.Fail(404, "Restaurant not found");
                return ApiResult.Ok(new
                {
                    x.BusinessHours,
                    x.RemainingParkingSpaces,
                    x.RestaurantId,
                    x.RestaurantName,
                    x.Address,
                    x.Phone,
                    rating = x.Rating.ToString(),
                    x.PriceRange,
                    x.RestaurantType,
                    x.ImageUrl,
                    x.Description,
                    distance = x.Distance.ToString(),
                    businessType = "restaurant"
                });
            }
    }
}
