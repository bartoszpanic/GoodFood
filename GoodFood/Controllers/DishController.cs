using GoodFood.Models;
using GoodFood.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodFood.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    [Authorize]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpDelete("{dishId}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Delete([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            _dishService.Remove(restaurantId, dishId);
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult DeleteAll([FromRoute] int restaurantId)
        {
            _dishService.RemoveAll(restaurantId);

            return NoContent();
        }

        [HttpPut("{dishId}")]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Update([FromRoute] int restaurantId, [FromRoute] int dishId, [FromBody] UpdateDishDto dto)
        {
            _dishService.Update(restaurantId, dishId, dto);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Post([FromRoute] int restaurantId,[FromBody] CreateDishDto dto)
        {
            var newDishId = _dishService.Create(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            DishDto dish = _dishService.GetById(restaurantId, dishId);

            return Ok(dish);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<DishDto>> GetAll([FromRoute] int restaurantId)
        {
            var result = _dishService.GetAll(restaurantId);

            return Ok(result);
        }
    }
}
