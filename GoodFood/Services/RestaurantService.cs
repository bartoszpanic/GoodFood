using AutoMapper;
using GoodFood.Entities;
using GoodFood.Exceptions;
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

        public async Task UpdateAsync(int id,UpdateRestaurantDto dto)
        {
            var restaurant = await _db
                .Restaurants
                .FirstOrDefaultAsync(r => r.Id == id);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogError($"Restaurant with id : {id}, DELETE action invoked");

            var restaurant = await _db
                .Restaurants
                .FirstOrDefaultAsync(r => r.Id == id);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            _db.Restaurants.Remove(restaurant);
            await _db.SaveChangesAsync();
        }

        public async Task<RestaurantDto> GetByIdAsync(int id)
        {
            var restaurant = await _db
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefaultAsync(r => r.Id == id);

            if(restaurant is null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            var result = _mapper.Map<RestaurantDto>(restaurant);

            return result;
        }

        public async Task<IEnumerable<RestaurantDto>> GetAllAsync()
        {
            var restaurants = await _db
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .ToListAsync();

            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

            return restaurantsDtos;
        }

        public async Task<int> CreateAsync(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            await _db.Restaurants.AddAsync(restaurant);
            await _db.SaveChangesAsync();

            return restaurant.Id;
        }
    }
}
