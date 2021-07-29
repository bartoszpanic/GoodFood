using AutoMapper;
using GoodFood.Entities;
using GoodFood.Exceptions;
using GoodFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodFood.Services
{
    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public DishService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = _db.Restaurants.FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var dishEntity = _mapper.Map<Dish>(dto);

            dishEntity.RestaurantId = restaurantId;
            
            _db.Dishes.Add(dishEntity);
            _db.SaveChanges();

            return dishEntity.Id;
        }
    }
}
