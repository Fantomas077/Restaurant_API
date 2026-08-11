using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Services;
using Restaurants.Infrastructure.Persistence;

namespace Restaurant.API.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllRestaurant()
        {
            var restaurant = await restaurantsService.GetAllRestaurants();
            return Ok(restaurant);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurantById([FromRoute] int id)
        {
            var restaurant = await restaurantsService.GetById(id);
            if (restaurant is null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var restaurant = await restaurantsService.Create(dto);

            return CreatedAtAction(
                nameof(GetRestaurantById),
                new { id = restaurant.Id },
                restaurant);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteRestaurant([FromRoute]int id)
        {
            var delete = await restaurantsService.DeleteRestaurant(id);
            if(!delete)
            {
                return NotFound($" Restaurant with Id {id} not found");
            }
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant( int id,[FromBody]UpdateRestaurantDto dto)
        {
            var restaurant = await restaurantsService.Update(id, dto);

            if (restaurant == null)
            {
                return NotFound($"Restaurant with id {id} not found");
            }

            return Ok(restaurant);
        }
    }
}
