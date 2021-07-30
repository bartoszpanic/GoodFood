using GoodFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodFood.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
        DishDto GetById(int restaurantId, int dishId);
        List<DishDto> GetAll(int restaurantId);
        void Update(int restaurantId, int dishId, UpdateDishDto dto);
        void RemoveAll(int restaurantId);
        void Remove(int restaurantId, int dishId);

    }
}
