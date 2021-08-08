using AutoMapper;
using GoodFood.Authorization;
using GoodFood.Entities;
using GoodFood.Exceptions;
using GoodFood.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoodFood.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        public RestaurantService(ApplicationDbContext db, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
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

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
                new ResourceOperationRequirement(Operation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
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

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
                new ResourceOperationRequirement(Operation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
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

        public async Task<RestaurantPagedResult<RestaurantDto>> GetAllAsync(RestaurantQuery query)
        {
            var baseQuery = _db
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .Where(r => query.SearchPhrase == null || (r.Name.ToLower().Contains(query.SearchPhrase.ToLower())
                                                       || r.Description.ToLower().Contains(query.SearchPhrase.ToLower())));

            var restaurants = await baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToListAsync();

            var totalItemsCount = baseQuery.Count();

            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

            var result = new RestaurantPagedResult<RestaurantDto>(restaurantsDtos,totalItemsCount, query.PageSize, query.PageNumber);

            return result;
        }

        public async Task<int> CreateAsync(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.GetUserId;
            await _db.Restaurants.AddAsync(restaurant);
            await _db.SaveChangesAsync();

            return restaurant.Id;
        }
    }
}
