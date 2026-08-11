using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Services;

namespace Restaurant.API.Controllers
{
    [Route("api/restaurants/{restaurantId}/dishes")]
    [ApiController]
    public class DishesController(IDishesService dishesService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetRestaurantDishes(
            [FromRoute] int restaurantId)
        {
            var dishes = await dishesService.GetAllDishes(restaurantId);

            if (dishes is null)
            {
                return NotFound();
            }

            return Ok(dishes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDish(
            [FromRoute] int restaurantId,
            [FromBody] CreateDishDto dto)
        {
            var dish = await dishesService.Create(restaurantId, dto);

            if (dish is null)
            {
                return NotFound(
                    $"Restaurant with ID {restaurantId} was not found.");
            }

            return Ok(dish);
        }
        [HttpDelete("{dishId}")]
        public async Task<IActionResult> DeleteDish([FromRoute] int restaurantId ,[FromRoute] int dishId)
        {
            var deleted = await dishesService.Delete(
                restaurantId,
                dishId);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpPut("{dishId}")]
        public async Task<IActionResult> UpdateDish( [FromRoute] int restaurantId,[FromRoute] int dishId,[FromBody] UpdateDishDto dto)
        {
            var dish = await dishesService.Update( restaurantId,dto, dishId);

            if (dish is null)
            {
                return NotFound();
            }

            return Ok(dish);
        }
    }


}