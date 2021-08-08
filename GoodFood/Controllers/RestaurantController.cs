using GoodFood.Entities;
using GoodFood.Models;
using GoodFood.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoodFood.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> UpdateAsync([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
        {
            await _restaurantService.UpdateAsync(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            await _restaurantService.DeleteAsync(id);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var id = await _restaurantService.CreateAsync(dto);

            return Created($"/api/restaurant/{id}", null);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll([FromQuery] string searchPhrase)
        {
            var restaurantDtos = await _restaurantService.GetAllAsync(searchPhrase);

            return Ok(restaurantDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        //[Authorize(Policy = "AtLeast20")]
        public async Task<ActionResult<RestaurantDto>> Get([FromRoute] int id)
        {
            var restaurant = await _restaurantService.GetByIdAsync(id);

            return Ok(restaurant);
        }
    }
}
