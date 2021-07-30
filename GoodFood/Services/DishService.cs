using AutoMapper;
using GoodFood.Entities;
using GoodFood.Exceptions;
using GoodFood.Models;
using Microsoft.EntityFrameworkCore;
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
            var restaurant = GetRestaurantById(restaurantId);

            var dishEntity = _mapper.Map<Dish>(dto);

            dishEntity.RestaurantId = restaurantId;

            _db.Dishes.Add(dishEntity);
            _db.SaveChanges();

            return dishEntity.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _db.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish is null || dish.RestaurantId != restaurantId)
            {
                throw new NotFoundException("Dish not found");
            }

            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);
            return dishDtos;
        }

        public void Update(int restaurantId, int dishId, UpdateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _db.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish is null || dish.RestaurantId != restaurantId)
            {
                throw new NotFoundException("Dish not found");
            }

            dish.Name = dto.Name;
            dish.Description = dto.Description;
            dish.Price = dto.Price;

            _db.SaveChanges();
        }

        public void RemoveAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            _db.RemoveRange(restaurant.Dishes);
            _db.SaveChanges();
        }

        public void Remove(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = GetDishById(restaurantId, dishId);

            _db.Dishes.Remove(dish);
            _db.SaveChanges();
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _db
                .Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            return restaurant;
        }

        private Dish GetDishById(int dishId)
        {
            var dish = _db.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish is null)
            {
                throw new NotFoundException("Dish not found");
            }

            return dish;
        }

        private Dish GetDishById(int restaurantId,int dishId)
        {
            var dish = _db.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish is null || dish.RestaurantId != restaurantId)
            {
                throw new NotFoundException("Dish not found");
            }

            return dish;
        }

    }
}
