using AutoMapper;
using GoodFood.Entities;
using GoodFood.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodFood.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public RestaurantService(ApplicationDbContext db, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }


        public bool Update(int id,UpdateRestaurantDto dto)
        {
            var restaurant = _db
                .Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurant == null)
            {
                return false;
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            _db.SaveChanges();

            return true;
        }


        public bool Delete(int id)
        {
            _logger.LogError($"Restaurant with id : {id}, DELETE action invoked");

            var restaurant = _db
                .Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurant == null)
            {
                return false;
            }

            _db.Restaurants.Remove(restaurant);
            _db.SaveChanges();
            return true;
        }


        public RestaurantDto GetById(int id)
        {
            var restaurant = _db
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if(restaurant is null) return null;

            var result = _mapper.Map<RestaurantDto>(restaurant);

            return result;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = _db
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .ToList();

            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

            return restaurantsDtos;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            _db.Restaurants.Add(restaurant);
            _db.SaveChanges();

            return restaurant.Id;
        }
    }
}
